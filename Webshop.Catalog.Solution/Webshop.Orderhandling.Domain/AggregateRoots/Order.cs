using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Webshop.Catalog.Domain.AggregateRoots;
using Webshop.Domain.Common;

namespace Webshop.Orderhandling.Domain.AggregateRoots
{
    public class Order : AggregateRoot
    {
        public List<Product> Products { get; set; }
        public int Discount { get; set; } = 0;

        public Order() 
        {
            Products = new List<Product>();
        }
    }
}
