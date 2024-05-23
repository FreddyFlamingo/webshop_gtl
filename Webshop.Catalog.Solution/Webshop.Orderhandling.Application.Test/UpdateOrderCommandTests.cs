using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Webshop.Orderhandling.Application.Contracts.Persistence;
using Webshop.Orderhandling.Application.Features.Order.Commands.UpdateOrder;
using Webshop.Orderhandling.Domain.AggregateRoots;
using Webshop.Domain.Common;

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
            var order = new Order
            {
                Id = 1,
                CustomerId = "customer1",
                Discount = 5,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { ProductId = 1, Quantity = 1, UnitPrice = 10 }
                }
            };
            orderRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).ReturnsAsync(order);

            var command = new UpdateOrderCommand(1, "customer1", new List<UpdateOrderCommand.UpdateOrderItem>
            {
                new UpdateOrderCommand.UpdateOrderItem { ProductId = 1, Quantity = 2, UnitPrice = 10 }
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
            var command = new UpdateOrderCommand(1, "", new List<UpdateOrderCommand.UpdateOrderItem>(), 10); // Invalid command
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
            var order = new Order
            {
                Id = 1,
                CustomerId = "customer1",
                Discount = 5,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { ProductId = 1, Quantity = 1, UnitPrice = 10 }
                }
            };
            orderRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).ReturnsAsync(order);

            var command = new UpdateOrderCommand(1, "customer1", new List<UpdateOrderCommand.UpdateOrderItem>
            {
                new UpdateOrderCommand.UpdateOrderItem { ProductId = 1, Quantity = 2, UnitPrice = 10 }
            }, 20); // Invalid discount
            var handler = new UpdateOrderCommandHandler(loggerMock.Object, orderRepositoryMock.Object);

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("unspecified.error", result.Error.Code);
        }

        [Fact]
        public async Task UpdateOrderCommandHandler_EmptyOrderItems_ExpectFailure()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<UpdateOrderCommandHandler>>();
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var order = new Order
            {
                Id = 1,
                CustomerId = "customer1",
                Discount = 5,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { ProductId = 1, Quantity = 1, UnitPrice = 10 }
                }
            };
            orderRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).ReturnsAsync(order);

            var command = new UpdateOrderCommand(1, "customer1", new List<UpdateOrderCommand.UpdateOrderItem>(), 10); // No items
            var handler = new UpdateOrderCommandHandler(loggerMock.Object, orderRepositoryMock.Object);

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("value.empty", result.Error.Code);
        }
    }
}
