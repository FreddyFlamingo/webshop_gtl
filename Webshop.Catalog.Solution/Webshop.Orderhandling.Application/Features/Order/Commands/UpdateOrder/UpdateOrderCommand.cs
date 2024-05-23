using System.Collections.Generic;
using Webshop.Application.Contracts;

namespace Webshop.Orderhandling.Application.Features.Order.Commands.UpdateOrder
{
    public class UpdateOrderCommand : ICommand
    {
        public UpdateOrderCommand(int orderId, string customerId, List<UpdateOrderItem> orderItems, decimal discount)
        {
            OrderId = orderId;
            CustomerId = customerId;
            OrderItems = orderItems;
            Discount = discount;
        }

        public int OrderId { get; private set; }
        public string CustomerId { get; private set; }
        public List<UpdateOrderItem> OrderItems { get; private set; }
        public decimal Discount { get; private set; }

        public class UpdateOrderItem
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
        }
    }
}
