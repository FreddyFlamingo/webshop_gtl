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
using Webshop.Orderhandling.Application.Features.Order.Commands.DeleteOrder;
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
            var command = new DeleteOrderCommand(1);
            var handler = new DeleteOrderCommandHandler(loggerMock.Object, orderRepositoryMock.Object);

            // Act
            var result = await handler.Handle(command);

            // Assert
            orderRepositoryMock.Verify(m => m.DeleteAsync(1), Times.Once);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task DeleteOrderCommandHandler_OrderNotFound_ExpectFailure()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<DeleteOrderCommandHandler>>();
            var orderRepositoryMock = new Mock<IOrderRepository>();
            orderRepositoryMock.Setup(m => m.DeleteAsync(1)).ThrowsAsync(new Exception("Order not found"));
            var command = new DeleteOrderCommand(1);
            var handler = new DeleteOrderCommandHandler(loggerMock.Object, orderRepositoryMock.Object);

            // Act
            var result = await handler.Handle(command);

            // Assert
            Assert.False(result.Success);
        }
    }
}
