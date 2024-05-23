using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Webshop.Application.Contracts;
using Webshop.Orderhandling.Application.Contracts.Persistence;
using Webshop.Domain.Common;

namespace Webshop.Orderhandling.Application.Features.Order.Commands.DeleteOrder
{
    public class DeleteOrderCommandHandler : ICommandHandler<DeleteOrderCommand>
    {
        private readonly ILogger<DeleteOrderCommandHandler> logger;
        private readonly IOrderRepository orderRepository;

        public DeleteOrderCommandHandler(ILogger<DeleteOrderCommandHandler> logger, IOrderRepository orderRepository)
        {
            this.logger = logger;
            this.orderRepository = orderRepository;
        }

        public async Task<Result> Handle(DeleteOrderCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                var order = await orderRepository.GetById(command.OrderId);

                if (order == null)
                {
                    return Result.Fail(Errors.General.NotFound<int>(command.OrderId));
                }

                await orderRepository.DeleteAsync(order.Id);
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
