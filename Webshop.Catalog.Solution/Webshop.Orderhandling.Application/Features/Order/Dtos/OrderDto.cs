using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Webshop.Orderhandling.Application.Features.Order.Dtos
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Discount { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
    }

}
