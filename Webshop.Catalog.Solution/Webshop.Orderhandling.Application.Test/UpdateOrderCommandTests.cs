using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Webshop.Orderhandling.Application.Contracts.Persistence;
using Webshop.Orderhandling.Application.Features.Order.Commands.UpdateOrder;
using Webshop.Orderhandling.Domain.AggregateRoots;
using Webshop.Catalog.Domain.AggregateRoots;
using Webshop.Domain.Common;
using Webshop.Orderhandling.Application.Features.Order.Commands.CreateOrder;

namespace Webshop.Orderhandling.Application.Test
{
    public class UpdateOrderCommandTests
    {
        [Fact]
        public async Task UpdateOrderCommandHandler_ValidCommand_ExpectSuccess()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<UpdateOrderCommandHandler>>();
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var product = new Product("Test Product", "SKU123", 100, "DKK");
            var order = new Order
            {
                Id = 1,
                CustomerId = "customer1",
                Discount = 5,
                Products = new List<Product>
                {
                    new Product ()
                }
            };
            orderRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).ReturnsAsync(order);

            var command = new UpdateOrderCommand(1, "customer1", new List<Product>
            {
                new Product { Price = 10}
            }, 10);
            var handler = new UpdateOrderCommandHandler(loggerMock.Object, orderRepositoryMock.Object);

            // Act
            var result = await handler.Handle(command);

            // Assert
            orderRepositoryMock.Verify(m => m.UpdateAsync(It.IsAny<Order>()), Times.Once);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task UpdateOrderCommandHandler_InvalidCommand_ExpectFailure()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<UpdateOrderCommandHandler>>();
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var command = new UpdateOrderCommand(1, "", new List<Product>(), 10); // Invalid command
            var handler = new UpdateOrderCommandHandler(loggerMock.Object, orderRepositoryMock.Object);

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async Task UpdateOrderCommandHandler_DiscountOutOfRange_ExpectFailure()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<UpdateOrderCommandHandler>>();
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var product = new Product("Test Product", "SKU123", 100, "DKK");
            var order = new Order
            {
                Id = 1,
                CustomerId = "customer1",
                Discount = 5,
                Products = new List<Product>
                {
                    new Product ()
                }
            };
            orderRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).ReturnsAsync(order);

            var command = new UpdateOrderCommand(1, "customer1", new List<Product>
            {
                new Product { Price = 10}
            }, 16); // Invalid discount
            var handler = new UpdateOrderCommandHandler(loggerMock.Object, orderRepositoryMock.Object);

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Discount must be between 0% and 15%", result.Error.Message);
        }

        [Fact]
        public async Task UpdateOrderCommandHandler_EmptyOrderItems_ExpectFailure()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<UpdateOrderCommandHandler>>();
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var product = new Product("Test Product", "SKU123", 100, "DKK");
            var order = new Order
            {
                Id = 1,
                CustomerId = "customer1",
                Discount = 5,
                Products = new List<Product>
                {
                    new Product ()
                }
            };
            orderRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).ReturnsAsync(order);

            var command = new UpdateOrderCommand(1, "customer1", new List<Product>(), 10); // No items
            var handler = new UpdateOrderCommandHandler(loggerMock.Object, orderRepositoryMock.Object);

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("The value cannot be empty: Order must contain at least one item. ", result.Error.Message);
        }
    }
}
