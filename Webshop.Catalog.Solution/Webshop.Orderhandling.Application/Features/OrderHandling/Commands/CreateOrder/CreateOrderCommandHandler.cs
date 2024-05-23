using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Domain.Common;
using Webshop.Orderhandling.Application.Contracts;

namespace Webshop.Orderhandling.Application.Features.OrderHandling.Commands.CreateOrder
{
    public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand>
    {
        private ILogger<CreateOrderCommandHandler> _logger;
        private IOrderRepository _orderRepository;
        public CreateOrderCommandHandler(ILogger<CreateOrderCommandHandler> logger, IOrderRepository orderRepository)
        {
            _logger = logger;
            _orderRepository = orderRepository;
        }

        public async Task<Result> Handle(CreateOrderCommand command, CancellationToken cancellationToken = default)
        {
            return null;
        }
    }
}
