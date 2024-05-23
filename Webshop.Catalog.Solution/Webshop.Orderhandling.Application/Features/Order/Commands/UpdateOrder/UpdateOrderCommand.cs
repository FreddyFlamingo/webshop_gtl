using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;

namespace Webshop.Orderhandling.Application.Features.Order.Commands.UpdateOrder
{
    public class UpdateOrderCommand : ICommand
    {
        public UpdateOrderCommand(int orderId, List<UpdateOrderItem> orderItems)
        {
            OrderId = orderId;
            OrderItems = orderItems;
        }

        public int OrderId { get; private set; }
        public List<UpdateOrderItem> OrderItems { get; private set; }

        public class UpdateOrderItem
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
        }
    }
}
