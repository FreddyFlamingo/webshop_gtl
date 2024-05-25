using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Catalog.Domain.AggregateRoots;

namespace Webshop.Orderhandling.Application.Features.Order.Commands.CreateOrder
{
    public class CreateOrderCommand : ICommand
    {
        public CreateOrderCommand(string customerId, List<Product> products, decimal discount)
        {
            CustomerId = customerId;
            Products = products;
            Discount = discount;
        }

        public string CustomerId { get; private set; }
        public List<Product> Products { get; private set; }
        public decimal Discount { get; private set; }

    }
}
