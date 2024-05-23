using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Orderhandling.Domain.AggregateRoots;
using Webshop.Application.Contracts.Persistence;

namespace Webshop.Orderhandling.Application.Contracts.Persistence
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersByCustomerId(string customerId);
        Task<Order> GetOrderById(int orderId);
    }
}
