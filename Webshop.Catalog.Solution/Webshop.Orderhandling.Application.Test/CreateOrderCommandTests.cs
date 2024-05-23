using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Webshop.Orderhandling.Application.Contracts.Persistence;
using Webshop.Orderhandling.Application.Features.Order.Commands.CreateOrder;
using Webshop.Orderhandling.Domain.AggregateRoots;
using Webshop.Domain.Common;

namespace Webshop.Orderhandling.Application.Test
{
    public class CreateOrderCommandTests
    {
        [Fact]
        public async Task CreateOrderCommandHandler_ValidCommand_ExpectSuccess()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<CreateOrderCommandHandler>>();
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var command = new CreateOrderCommand("customer1", new List<CreateOrderCommand.CreateOrderItem>
            {
                new CreateOrderCommand.CreateOrderItem { ProductId = 1, Quantity = 2, UnitPrice = 10 }
            }, 10);
            var handler = new CreateOrderCommandHandler(loggerMock.Object, orderRepositoryMock.Object);

            // Act
            var result = await handler.Handle(command);

            // Assert
            orderRepositoryMock.Verify(m => m.CreateAsync(It.IsAny<Order>()), Times.Once);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task CreateOrderCommandHandler_DiscountAbove15Percent_ExpectFailure()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<CreateOrderCommandHandler>>();
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var command = new CreateOrderCommand("customer1", new List<CreateOrderCommand.CreateOrderItem>
            {
                new CreateOrderCommand.CreateOrderItem { ProductId = 1, Quantity = 2, UnitPrice = 10 }
            }, 20);
            var handler = new CreateOrderCommandHandler(loggerMock.Object, orderRepositoryMock.Object);

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Discount must be between 0% and 15%", result.Error.Message);
        }

        [Fact]
        public async Task CreateOrderCommandHandler_EmptyOrder_ExpectFailure()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<CreateOrderCommandHandler>>();
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var command = new CreateOrderCommand("customer1", new List<CreateOrderCommand.CreateOrderItem>(), 10);
            var handler = new CreateOrderCommandHandler(loggerMock.Object, orderRepositoryMock.Object);

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Order must contain at least one item.", result.Error.Message);
        }

        [Fact]
        public async Task CreateOrderCommandHandler_DiscountBelow0Percent_ExpectFailure()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<CreateOrderCommandHandler>>();
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var command = new CreateOrderCommand("customer1", new List<CreateOrderCommand.CreateOrderItem>
            {
                new CreateOrderCommand.CreateOrderItem { ProductId = 1, Quantity = 2, UnitPrice = 10 }
            }, -5);
            var handler = new CreateOrderCommandHandler(loggerMock.Object, orderRepositoryMock.Object);

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Discount must be between 0% and 15%", result.Error.Message);
        }
    }
}
