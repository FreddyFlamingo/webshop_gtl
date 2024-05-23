using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts.Persistence;
using Webshop.Domain.AggregateRoots;
using Webshop.Domain.Common;
using Webshop.Orderhandling.Domain.AggregateRoots;

namespace Webshop.Orderhandling.Application.Contracts
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<Result> CompleteOrder(Order order);
    }
}
