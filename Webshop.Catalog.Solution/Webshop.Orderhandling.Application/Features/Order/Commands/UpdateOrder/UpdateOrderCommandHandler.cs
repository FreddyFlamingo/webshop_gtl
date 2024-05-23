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
                var order = await this.orderRepository.GetOrderById(command.OrderId);
                if (order == null)
                {
                    return Result.Fail(Errors.General.NotFound(command.OrderId));
                }

                order.OrderItems.Clear();
                order.TotalAmount = 0;

                foreach (var item in command.OrderItems)
                {
                    var orderItem = new OrderItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    };

                    order.TotalAmount += item.Quantity * item.UnitPrice;
                    order.OrderItems.Add(orderItem);
                }

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
