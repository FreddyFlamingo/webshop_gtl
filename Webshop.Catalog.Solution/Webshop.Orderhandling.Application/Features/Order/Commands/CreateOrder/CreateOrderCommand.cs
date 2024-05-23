using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;

namespace Webshop.Orderhandling.Application.Features.Order.Commands.CreateOrder
{
    public class CreateOrderCommand : ICommand
    {
        public CreateOrderCommand(string customerId, List<CreateOrderItem> orderItems, decimal discount)
        {
            CustomerId = customerId;
            OrderItems = orderItems;
            Discount = discount;
        }

        public string CustomerId { get; private set; }
        public List<CreateOrderItem> OrderItems { get; private set; }
        public decimal Discount { get; private set; }

        public class CreateOrderItem
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
        }
    }
}
