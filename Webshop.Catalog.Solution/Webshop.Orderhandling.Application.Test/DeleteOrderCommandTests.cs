using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Webshop.Orderhandling.Application.Contracts.Persistence;
using Webshop.Orderhandling.Application.Features.Order.Commands.DeleteOrder;
using Webshop.Orderhandling.Domain.AggregateRoots;
using Webshop.Domain.Common;

namespace Webshop.Orderhandling.Application.Test
{
    public class DeleteOrderCommandTests
    {
        [Fact]
        public async Task DeleteOrderCommandHandler_ValidCommand_ExpectSuccess()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<DeleteOrderCommandHandler>>();
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var order = new Order { Id = 1, CustomerId = "customer1" };
            orderRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).ReturnsAsync(order);

            var command = new DeleteOrderCommand(1);
            var handler = new DeleteOrderCommandHandler(loggerMock.Object, orderRepositoryMock.Object);

            // Act
            var result = await handler.Handle(command);

            // Assert
            orderRepositoryMock.Verify(m => m.DeleteAsync(It.IsAny<int>()), Times.Once);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task DeleteOrderCommandHandler_InvalidCommand_ExpectFailure()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<DeleteOrderCommandHandler>>();
            var orderRepositoryMock = new Mock<IOrderRepository>();
            orderRepositoryMock.Setup(m => m.GetById(It.IsAny<int>())).ReturnsAsync((Order)null);

            var command = new DeleteOrderCommand(1);
            var handler = new DeleteOrderCommandHandler(loggerMock.Object, orderRepositoryMock.Object);

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("entity.not.found", result.Error.Code);
        }
    }
}
