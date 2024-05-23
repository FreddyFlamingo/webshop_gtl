using Microsoft.VisualBasic;
using Moq;
using System.Diagnostics.CodeAnalysis;
using Webshop.Catalog.Domain.AggregateRoots;
using Webshop.Domain.AggregateRoots;
using Webshop.Domain.Common;
using Webshop.Orderhandling.Application.Contracts;
using Webshop.Orderhandling.Application.Features.OrderHandling.Commands.CreateOrder;
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

        [Fact]
        public async Task OrderWithZeroDiscount_Will_PassAsync()
        {
            //arrange
            var loggerMock = new Mock<Microsoft.Extensions.Logging.ILogger<CreateOrderCommandHandler>>();
            var orderRepositoryMock = new Mock<IOrderRepository>();
            Order order = new Order();
            CreateOrderCommand command = new CreateOrderCommand(order.Products, order.Discount);
            CreateOrderCommandHandler handler = new CreateOrderCommandHandler(loggerMock.Object, orderRepositoryMock.Object);
            //act
            Result result = await handler.Handle(command);
            //assert
            orderRepositoryMock.Verify(m => m.CompleteOrder(order), Times.Once);
            Assert.True(result.Success);
        }
    }
}