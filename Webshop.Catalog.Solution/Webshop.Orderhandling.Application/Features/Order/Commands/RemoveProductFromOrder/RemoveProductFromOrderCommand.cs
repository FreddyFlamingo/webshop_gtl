using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;

namespace Webshop.Orderhandling.Application.Features.Order.Commands.RemoveProductFromOrder
{
    public class RemoveProductFromOrderCommand : ICommand
    {
        public RemoveProductFromOrderCommand(int orderId, int productId)
        {
            OrderId = orderId;
            ProductId = productId;
        }

        public int OrderId { get; private set; }
        public int ProductId { get; private set; }
    }
}
