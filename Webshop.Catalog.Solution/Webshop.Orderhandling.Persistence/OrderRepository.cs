using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webshop.Orderhandling.Application.Contracts.Persistence;
using Webshop.Orderhandling.Domain.AggregateRoots;

namespace Webshop.Orderhandling.Persistence
{
    public class OrderRepository : IOrderRepository
    {
        private readonly List<Order> orders = new();

        public Task CreateAsync(Order entity)
        {
            entity.Id = orders.Any() ? orders.Max(o => o.Id) + 1 : 1;
            orders.Add(entity);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(int id)
        {
            var order = orders.SingleOrDefault(o => o.Id == id);
            if (order != null)
            {
                orders.Remove(order);
            }
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Order>> GetAll()
        {
            return Task.FromResult<IEnumerable<Order>>(orders);
        }

        public Task<Order> GetById(int id)
        {
            var order = orders.SingleOrDefault(o => o.Id == id);
            return Task.FromResult(order);
        }

        public Task<Order> GetOrderById(int orderId)
        {
            var order = orders.SingleOrDefault(o => o.Id == orderId);
            return Task.FromResult(order);
        }

        public Task<IEnumerable<Order>> GetOrdersByCustomerId(string customerId)
        {
            var customerOrders = orders.Where(o => o.CustomerId == customerId);
            return Task.FromResult<IEnumerable<Order>>(customerOrders);
        }

        public Task UpdateAsync(Order entity)
        {
            var order = orders.SingleOrDefault(o => o.Id == entity.Id);
            if (order != null)
            {
                order.OrderDate = entity.OrderDate;
                order.CustomerId = entity.CustomerId;
                order.TotalAmount = entity.TotalAmount;
                order.OrderItems = entity.OrderItems;
                order.Discount = entity.Discount;
            }

            return Task.CompletedTask;
        }
    }
}
