using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Webshop.Catalog.Domain.AggregateRoots;
using Webshop.Domain.Common;

namespace Webshop.Orderhandling.Application.Features.Order.Requests
{
    public class UpdateOrderRequest
    {
        public string CustomerId { get; set; }
        public int OrderId { get; set; }
        public List<UpdateOrderItem> OrderItems { get; set; }
        public decimal Discount { get; set; }

        public class UpdateOrderItem
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
        }

        public class Validator : AbstractValidator<UpdateOrderRequest>
        {
            public Validator()
            {
                RuleFor(r => r.CustomerId).NotEmpty().WithMessage(Errors.General.ValueIsEmpty(nameof(CustomerId)).Message);
                RuleForEach(r => r.OrderItems).ChildRules(orderItem =>
                {
                    orderItem.RuleFor(r => r.ProductId)
                        .GreaterThan(0)
                        .WithMessage(Errors.General.ValueTooSmall(nameof(UpdateOrderItem.ProductId), 0).Message);
                    orderItem.RuleFor(r => r.Quantity)
                        .GreaterThan(0)
                        .WithMessage(Errors.General.ValueTooSmall(nameof(UpdateOrderItem.Quantity), 0).Message);
                    orderItem.RuleFor(r => r.UnitPrice)
                        .GreaterThan(0)
                        .WithMessage(Errors.General.ValueTooSmall(nameof(UpdateOrderItem.UnitPrice), 0).Message);
                });
                RuleFor(r => r.Discount).InclusiveBetween(0, 15).WithMessage(Errors.General.ValueOutOfRange(nameof(Discount), 0, 15).Message);
            }
        }
    }
}
