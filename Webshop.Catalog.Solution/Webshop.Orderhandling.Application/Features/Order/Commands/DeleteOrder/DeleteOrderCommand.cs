using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;

namespace Webshop.Orderhandling.Application.Features.Order.Commands.DeleteOrder
{
    public class DeleteOrderCommand : ICommand
    {
        public DeleteOrderCommand(int orderId)
        {
            OrderId = orderId;
        }

        public int OrderId { get; private set; }
    }
}
