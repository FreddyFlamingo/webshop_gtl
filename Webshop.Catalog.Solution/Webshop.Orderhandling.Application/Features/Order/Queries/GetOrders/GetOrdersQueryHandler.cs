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

namespace Webshop.Orderhandling.Application.Features.Order.Queries.GetOrders
{
    public class GetOrdersQueryHandler : IQueryHandler<GetOrdersQuery, List<OrderDto>>
    {
        private readonly ILogger<GetOrdersQueryHandler> logger;
        private readonly IMapper mapper;
        private readonly IOrderRepository orderRepository;

        public GetOrdersQueryHandler(ILogger<GetOrdersQueryHandler> logger, IMapper mapper, IOrderRepository orderRepository)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.orderRepository = orderRepository;
        }

        public async Task<Result<List<OrderDto>>> Handle(GetOrdersQuery query, CancellationToken cancellationToken = default)
        {
            try
            {
                var orders = await this.orderRepository.GetOrdersByCustomerId(query.CustomerId);
                var orderDtos = this.mapper.Map<List<OrderDto>>(orders);
                return Result.Ok(orderDtos);
            }
            catch (Exception ex)
            {
                this.logger.LogCritical(ex, ex.Message);
                return Result.Fail<List<OrderDto>>(Errors.General.UnspecifiedError(ex.Message));
            }
        }
    }
}
