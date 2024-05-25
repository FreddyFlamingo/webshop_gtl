using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Webshop.Application.Contracts;
using Webshop.Orderhandling.Application.Contracts.Persistence;
using Webshop.Orderhandling.Domain.AggregateRoots;
using Webshop.Domain.Common;
using Webshop.Catalog.Domain.AggregateRoots;

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
                if (command.Products == null || !command.Products.Any())
                {
                    return Result.Fail(Errors.General.ValueIsEmpty("Order must contain at least one item."));
                }

                var order = await orderRepository.GetById(command.OrderId);

                if (order == null)
                {
                    return Result.Fail(Errors.General.NotFound<int>(command.OrderId));
                }

                order.CustomerId = command.CustomerId;
                order.Products.Clear();
                order.TotalAmount = 0;

                foreach (var item in command.Products)
                {
                    var product = new Product
                    {
                    };

                    order.Products.Add(product);
                    order.TotalAmount += item.Price;
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
