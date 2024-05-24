using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Webshop.Application.Contracts;
using Webshop.Orderhandling.Application.Contracts.Persistence;
using Webshop.Orderhandling.Domain.AggregateRoots;
using Webshop.Domain.Common;

namespace Webshop.Orderhandling.Application.Features.Order.Commands.CreateOrder
{
    public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand>
    {
        private readonly ILogger<CreateOrderCommandHandler> logger;
        private readonly IOrderRepository orderRepository;

        public CreateOrderCommandHandler(ILogger<CreateOrderCommandHandler> logger, IOrderRepository orderRepository)
        {
            this.logger = logger;
            this.orderRepository = orderRepository;
        }

        public async Task<Result> Handle(CreateOrderCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                if (command.OrderItems == null || command.OrderItems.Count == 0)
                {
                    return Result.Fail(Errors.General.ValueIsEmpty("Order must contain at least one item."));
                }

                var order = new Webshop.Orderhandling.Domain.AggregateRoots.Order
                {
                    OrderDate = DateTime.UtcNow,
                    CustomerId = command.CustomerId,
                    TotalAmount = 0,
                    Discount = command.Discount,
                };

                foreach (var item in command.OrderItems)
                {
                    var orderItem = new OrderItem
                    {
                        Product = item.Product,
                        Quantity = item.Quantity
                    };

                    order.TotalAmount += item.TotalPrice;
                    order.OrderItems.Add(orderItem);
                }

                order.ApplyDiscount(command.Discount);

                await this.orderRepository.CreateAsync(order);
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
