using Microsoft.VisualBasic;
using System.Diagnostics.CodeAnalysis;
using Webshop.Catalog.Domain.AggregateRoots;
using Webshop.Orderhandling.Domain.AggregateRoots;

namespace Webshop.Orderhandling.Application.Test
{
    public class OrderTests
    {
        [Fact]
        public void AddProductToOrder_Will_Pass()
        {
            Order order = new Order();

            Product product = new Product();

            order.Products.Add(product);

            Assert.True(order.Products.Contains(product));
        }
    }
}