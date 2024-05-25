using System.Collections.Generic;
using Webshop.Application.Contracts;
using Webshop.Catalog.Domain.AggregateRoots;

namespace Webshop.Orderhandling.Application.Features.Order.Commands.UpdateOrder
{
    public class UpdateOrderCommand : ICommand
    {
        public UpdateOrderCommand(int orderId, string customerId, List<Product> products, decimal discount)
        {
            OrderId = orderId;
            CustomerId = customerId;
            Products = products;
            Discount = discount;
        }

        public int OrderId { get; private set; }
        public string CustomerId { get; private set; }
        public List<Product> Products { get; private set; }
        public decimal Discount { get; private set; }

    }
}
