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

namespace Webshop.Orderhandling.Application.Features.Order.Commands.AddProductToOrder
{
    public class AddProductToOrderCommandHandler : ICommandHandler<AddProductToOrderCommand>
    {
        private readonly ILogger<AddProductToOrderCommandHandler> logger;
        private readonly IOrderRepository orderRepository;

        public AddProductToOrderCommandHandler(ILogger<AddProductToOrderCommandHandler> logger, IOrderRepository orderRepository)
        {
            this.logger = logger;
            this.orderRepository = orderRepository;
        }

        public async Task<Result> Handle(AddProductToOrderCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                var order = await this.orderRepository.GetOrderById(command.OrderId);
                if (order == null)
                {
                    return Result.Fail(Errors.General.NotFound(command.OrderId));
                }

                var existingOrderItem = order.OrderItems.FirstOrDefault(i => i.ProductId == command.ProductId);
                if (existingOrderItem != null)
                {
                    existingOrderItem.Quantity += command.Quantity;
                    existingOrderItem.UnitPrice = command.UnitPrice;
                }
                else
                {
                    var orderItem = new OrderItem
                    {
                        ProductId = command.ProductId,
                        Quantity = command.Quantity,
                        UnitPrice = command.UnitPrice
                    };
                    order.OrderItems.Add(orderItem);
                }

                order.TotalAmount += command.Quantity * command.UnitPrice;

                await this.orderRepository.UpdateAsync(order);
                return Result.Ok();
            }
            catch (Exception ex)
            {
                this.logger.LogCritical(ex, ex.Message);
                return Result.Fail(Errors.General.UnspecifiedError(ex.Message));
            }
        }
    }
}
