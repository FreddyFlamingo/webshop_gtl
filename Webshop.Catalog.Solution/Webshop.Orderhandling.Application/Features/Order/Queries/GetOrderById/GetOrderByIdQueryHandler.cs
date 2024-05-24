using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Webshop.Application.Contracts;
using Webshop.Orderhandling.Application.Contracts.Persistence;
using Webshop.Orderhandling.Application.Features.Order.Dtos;
using Webshop.Domain.Common;

namespace Webshop.Orderhandling.Application.Features.Order.Queries.GetOrderById
{
    public class GetOrderByIdQueryHandler : IQueryHandler<GetOrderByIdQuery, OrderDto>
    {
        private readonly ILogger<GetOrderByIdQueryHandler> logger;
        private readonly IMapper mapper;
        private readonly IOrderRepository orderRepository;

        public GetOrderByIdQueryHandler(ILogger<GetOrderByIdQueryHandler> logger, IMapper mapper, IOrderRepository orderRepository)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.orderRepository = orderRepository;
        }

        public async Task<Result<OrderDto>> Handle(GetOrderByIdQuery query, CancellationToken cancellationToken = default)
        {
            try
            {
                var order = await this.orderRepository.GetOrderById(query.OrderId);
                if (order == null)
                {
                    return Result.Fail<OrderDto>(Errors.General.NotFound(query.OrderId));
                }

                var orderDto = this.mapper.Map<OrderDto>(order);
                return Result.Ok(orderDto);
            }
            catch (Exception ex)
            {
                this.logger.LogCritical(ex, ex.Message);
                return Result.Fail<OrderDto>(Errors.General.UnspecifiedError(ex.Message));
            }
        }
    }
}
