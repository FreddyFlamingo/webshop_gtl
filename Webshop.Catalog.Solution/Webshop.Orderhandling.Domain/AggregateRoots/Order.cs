using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Webshop.Catalog.Domain.AggregateRoots;

namespace Webshop.Orderhandling.Domain.AggregateRoots
{
    public class Order
    {
        public List<Product> Products { get; set; }

        public Order() 
        {
            Products = new List<Product>();
        }
    }
}
