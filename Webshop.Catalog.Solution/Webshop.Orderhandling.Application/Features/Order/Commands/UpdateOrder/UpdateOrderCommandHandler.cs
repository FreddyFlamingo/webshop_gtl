using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Webshop.Application.Contracts;
using Webshop.Orderhandling.Application.Contracts.Persistence;
using Webshop.Orderhandling.Domain.AggregateRoots;
using Webshop.Domain.Common;

namespace Webshop.Orderhandling.Application.Features.Order.Commands.UpdateOrder
{
    public class UpdateOrderCommandHandler : ICommandHandler<UpdateOrderCommand>
    {
        private readonly ILogger<UpdateOrderCommandHandler> logger;
        private readonly IOrderRepository orderRepository;

        public UpdateOrderCommandHandler(ILogger<UpdateOrderCommandHandler> logger, IOrderRepository orderRepository)
        {
            this.logger = logger;
            this.orderRepository = orderRepository;
        }

        public async Task<Result> Handle(UpdateOrderCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                // Validate non-empty order items
                if (command.OrderItems == null || !command.OrderItems.Any())
                {
                    return Result.Fail(Errors.General.ValueIsEmpty("Order must contain at least one item."));
                }

                var order = await orderRepository.GetById(command.OrderId);

                if (order == null)
                {
                    return Result.Fail(Errors.General.NotFound<int>(command.OrderId));
                }

                order.CustomerId = command.CustomerId;
                order.OrderItems.Clear();
                order.TotalAmount = 0;

                foreach (var item in command.OrderItems)
                {
                    var orderItem = new OrderItem
                    {
                        Product = item.Product,
                        Quantity = item.Quantity,
                    };

                    order.OrderItems.Add(orderItem);
                    order.TotalAmount += item.Quantity * item.Product.Price;
                }

                order.ApplyDiscount(command.Discount);

                await orderRepository.UpdateAsync(order);
                return Result.Ok();
            }
            catch (ArgumentException ex)
            {
                this.logger.LogError(ex, ex.Message);
                return Result.Fail(Errors.General.UnspecifiedError(ex.Message));
            }
            catch (Exception ex)
            {
                this.logger.LogCritical(ex, ex.Message);
                return Result.Fail(Errors.General.UnspecifiedError(ex.Message));
            }
        }
    }
}
