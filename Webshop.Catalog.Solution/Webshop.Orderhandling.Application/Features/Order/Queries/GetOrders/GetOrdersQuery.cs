using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Orderhandling.Application.Features.Order.Dtos;

namespace Webshop.Orderhandling.Application.Features.Order.Queries.GetOrders
{
    public class GetOrdersQuery : IQuery<List<OrderDto>>
    {
        public GetOrdersQuery(string customerId)
        {
            CustomerId = customerId;
        }

        public string CustomerId { get; private set; }
    }
}
