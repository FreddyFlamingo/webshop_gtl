using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Orderhandling.Application.Features.Order.Dtos;

namespace Webshop.Orderhandling.Application.Features.Order.Queries.GetOrderById
{
    public class GetOrderByIdQuery : IQuery<OrderDto>
    {
        public GetOrderByIdQuery(int orderId)
        {
            OrderId = orderId;
        }

        public int OrderId { get; private set; }
    }
}
