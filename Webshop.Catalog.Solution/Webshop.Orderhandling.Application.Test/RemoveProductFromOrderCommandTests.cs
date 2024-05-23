using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Webshop.Orderhandling.Application.Contracts.Persistence;
using Webshop.Orderhandling.Application.Features.Order.Commands.RemoveProductFromOrder;
using Webshop.Orderhandling.Domain.AggregateRoots;
using Webshop.Domain.Common;

namespace Webshop.Orderhandling.Application.Test
{
    public class RemoveProductFromOrderCommandTests
    {
        [Fact]
        public async Task RemoveProductFromOrderCommandHandler_ValidCommand_ExpectSuccess()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<RemoveProductFromOrderCommandHandler>>();
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var existingOrder = new Order { Id = 1, CustomerId = "customer1", OrderItems = new List<OrderItem> { new OrderItem { ProductId = 1, Quantity = 2, UnitPrice = 10 } } };
            orderRepositoryMock.Setup(m => m.GetOrderById(1)).ReturnsAsync(existingOrder);
            var command = new RemoveProductFromOrderCommand(1, 1);
            var handler = new RemoveProductFromOrderCommandHandler(loggerMock.Object, orderRepositoryMock.Object);

            // Act
            var result = await handler.Handle(command);

            // Assert
            orderRepositoryMock.Verify(m => m.UpdateAsync(It.IsAny<Order>()), Times.Once);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task RemoveProductFromOrderCommandHandler_ProductNotFound_ExpectFailure()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<RemoveProductFromOrderCommandHandler>>();
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var existingOrder = new Order { Id = 1, CustomerId = "customer1", OrderItems = new List<OrderItem>() };
            orderRepositoryMock.Setup(m => m.GetOrderById(1)).ReturnsAsync(existingOrder);
            var command = new RemoveProductFromOrderCommand(1, 1);
            var handler = new RemoveProductFromOrderCommandHandler(loggerMock.Object, orderRepositoryMock.Object);

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async Task RemoveProductFromOrderCommandHandler_OrderNotFound_ExpectFailure()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<RemoveProductFromOrderCommandHandler>>();
            var orderRepositoryMock = new Mock<IOrderRepository>();
            orderRepositoryMock.Setup(m => m.GetOrderById(1)).ReturnsAsync((Order)null);
            var command = new RemoveProductFromOrderCommand(1, 1);
            var handler = new RemoveProductFromOrderCommandHandler(loggerMock.Object, orderRepositoryMock.Object);

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.False(result.Success);
        }
    }
}
