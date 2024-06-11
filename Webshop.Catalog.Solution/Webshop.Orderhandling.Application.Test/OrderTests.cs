using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Webshop.Orderhandling.Application.Contracts.Persistence;
using Webshop.Orderhandling.Application.Features.Order.Commands.CreateOrder;
using Webshop.Orderhandling.Domain.AggregateRoots;
using Webshop.Catalog.Domain.AggregateRoots;
using Webshop.Domain.Common;

namespace Webshop.Orderhandling.Application.Test
{
    public class OrderTests
    {

        [Theory]
        [InlineData(-0.1)]
        [InlineData(15.1)]
        public void ApplyDiscount_InvalidDiscount_ExpectException(decimal discount)
        {
            // Arrange
            var order = new Order
            {
                Id = 1,
                CustomerId = "customer1",
                Discount = 5,
                Products = new List<Product> { new Product { Price = 10 } }
            };

            var originalTotalAmount = order.TotalAmount;

            // Act
            var result = order.ApplyDiscount(discount);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(originalTotalAmount, order.TotalAmount);
        }

    }
}
