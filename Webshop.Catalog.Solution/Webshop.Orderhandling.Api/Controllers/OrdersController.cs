using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Orderhandling.Application.Features.Order.Commands.CreateOrder;
using Webshop.Orderhandling.Application.Features.Order.Commands.UpdateOrder;
using Webshop.Orderhandling.Application.Features.Order.Dtos;
using Webshop.Orderhandling.Application.Features.Order.Queries.GetOrders;
using Webshop.Orderhandling.Application.Features.Order.Requests;
using Webshop.Domain.Common;
using Webshop.Orderhandling.Application.Features.Order.Commands.DeleteOrder;

namespace Webshop.Orderhandling.Api.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : BaseController
    {
        private readonly IDispatcher dispatcher;
        private readonly IMapper mapper;
        private readonly ILogger<OrdersController> logger;

        public OrdersController(IDispatcher dispatcher, IMapper mapper, ILogger<OrdersController> logger)
        {
            this.dispatcher = dispatcher;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            var validator = new CreateOrderRequest.Validator();
            var validationResult = await validator.ValidateAsync(request);
            if (validationResult.IsValid)
            {
                var command = mapper.Map<CreateOrderCommand>(request);
                var result = await dispatcher.Dispatch(command);
                return FromResult(result);
            }
            else
            {
                logger.LogError(string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage)));
                return Error(validationResult.Errors);
            }
        }

        [HttpGet]
        [Route("{customerId}")]
        public async Task<IActionResult> GetOrders(string customerId)
        {
            var query = new GetOrdersQuery(customerId);
            var result = await dispatcher.Dispatch(query);
            if (result.Success)
            {
                var ordersResult = result as Result<List<OrderDto>>;
                return FromResult(ordersResult);
            }
            else
            {
                logger.LogError(result.Error.Message);
                return Error(result.Error);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrderRequest request)
        {
            var validator = new UpdateOrderRequest.Validator();
            var validationResult = await validator.ValidateAsync(request);
            if (validationResult.IsValid)
            {
                var command = mapper.Map<UpdateOrderCommand>(request);
                var result = await dispatcher.Dispatch(command);
                return FromResult(result);
            }
            else
            {
                logger.LogError(string.Join(",", validationResult.Errors.Select(x => x.ErrorMessage)));
                return Error(validationResult.Errors);
            }
        }

        [HttpDelete]
        [Route("{orderId}")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            var command = new DeleteOrderCommand(orderId);
            var result = await dispatcher.Dispatch(command);
            return FromResult(result);
        }
    }
}
