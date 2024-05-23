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

namespace Webshop.Orderhandling.Application.Features.Order.Commands.RemoveProductFromOrder
{
    public class RemoveProductFromOrderCommandHandler : ICommandHandler<RemoveProductFromOrderCommand>
    {
        private readonly ILogger<RemoveProductFromOrderCommandHandler> logger;
        private readonly IOrderRepository orderRepository;

        public RemoveProductFromOrderCommandHandler(ILogger<RemoveProductFromOrderCommandHandler> logger, IOrderRepository orderRepository)
        {
            this.logger = logger;
            this.orderRepository = orderRepository;
        }

        public async Task<Result> Handle(RemoveProductFromOrderCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                var order = await this.orderRepository.GetOrderById(command.OrderId);
                if (order == null)
                {
                    return Result.Fail(Errors.General.NotFound(command.OrderId));
                }

                var orderItem = order.OrderItems.FirstOrDefault(i => i.ProductId == command.ProductId);
                if (orderItem != null)
                {
                    order.TotalAmount -= orderItem.Quantity * orderItem.UnitPrice;
                    order.OrderItems.Remove(orderItem);
                }
                else
                {
                    return Result.Fail(Errors.General.NotFound(command.OrderId));
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
