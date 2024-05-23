using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Webshop.Catalog.Domain.AggregateRoots;
using Webshop.Application.Contracts;

namespace Webshop.Orderhandling.Application.Features.OrderHandling.Commands.CreateOrder
{

    public class CreateOrderCommand : Webshop.Application.Contracts.ICommand
    {
        public CreateOrderCommand(List<Product> products, int discount)
        {
            Products = products;
            Discount = discount;

        }

        public List<Product> Products { get; private set; }
        public int Discount { get; set; }

        public event EventHandler? CanExecuteChanged;

    }
}
