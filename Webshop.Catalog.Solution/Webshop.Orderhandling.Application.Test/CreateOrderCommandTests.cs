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
    public class CreateOrderCommandTests
    {
        [Fact]
        public async Task CreateOrderCommandHandler_OneProduct_ValidCommand_ExpectSuccess()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<CreateOrderCommandHandler>>();
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var product = new Product("Test Product", "SKU123", 100, "DKK");
            var command = new CreateOrderCommand("customer1", new List<Product> { product }, 10);
            var handler = new CreateOrderCommandHandler(loggerMock.Object, orderRepositoryMock.Object);

            // Act
            var result = await handler.Handle(command);

            // Assert
            orderRepositoryMock.Verify(m => m.CreateAsync(It.IsAny<Order>()), Times.Once);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task CreateOrderCommandHandler_NoProduct_InvalidCommand_ExpectFailure()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<CreateOrderCommandHandler>>();
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var command = new CreateOrderCommand("customer1", new List<Product>(), 10);
            var handler = new CreateOrderCommandHandler(loggerMock.Object, orderRepositoryMock.Object);

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("The value cannot be empty: Order must contain at least one item. ", result.Error.Message);
        }

        //[Theory]
        //[InlineData(-0.1)]
        //[InlineData(15.1)]
        //public async Task CreateOrderCommandHandler_InvalidDiscount_ExpectFailure(decimal discount)
        //{
        //    // Arrange
        //    var loggerMock = new Mock<ILogger<CreateOrderCommandHandler>>();
        //    var orderRepositoryMock = new Mock<IOrderRepository>();
        //    var product = new Product("Test Product", "SKU123", 100, "DKK");
        //    var command = new CreateOrderCommand("customer1", new List<Product>() { product }, discount);
        //    var handler = new CreateOrderCommandHandler(loggerMock.Object, orderRepositoryMock.Object);

        //    // Act
        //    var result = await handler.Handle(command);

        //    // Assert
        //    Assert.False(result.Success);
        //    Assert.Equal("Discount must be between 0% and 15%", result.Error.Message);
        //}

        [Theory]
        [InlineData(0)]
        [InlineData(15)]
        public async Task CreateOrderCommandHandler_ValidDiscount_ExpectSuccess(decimal discount)
        {
            // Arrange
            var loggerMock = new Mock<ILogger<CreateOrderCommandHandler>>();
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var product = new Product("Test Product", "SKU123", 100, "DKK");
            var command = new CreateOrderCommand("customer1", new List<Product>() { product }, discount);
            var handler = new CreateOrderCommandHandler(loggerMock.Object, orderRepositoryMock.Object);

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.True(result.Success);
        }
    }
}
