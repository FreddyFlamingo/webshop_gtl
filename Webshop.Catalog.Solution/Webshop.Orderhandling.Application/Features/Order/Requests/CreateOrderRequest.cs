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
    public class CreateOrderRequest
    {
        public string CustomerId { get; set; }
        public List<CreateOrderItem> OrderItems { get; set; }
        public int Discount { get; set; }

        public class CreateOrderItem
        {
            public Product Product { get; set; }
            public int Quantity { get; set; }
            public decimal TotalPrice { get; set; }
        }

        public class Validator : AbstractValidator<CreateOrderRequest>
        {
            public Validator()
            {
                RuleFor(r => r.CustomerId).NotEmpty().WithMessage(Errors.General.ValueIsEmpty(nameof(CustomerId)).Message);
                RuleForEach(r => r.OrderItems).ChildRules(orderItem =>
                {
                    orderItem.RuleFor(r => r.Product.Id)
                        .NotEmpty()
                        .WithMessage(Errors.General.ValueIsEmpty(nameof(CreateOrderItem.Product.Id)).Message);
                    orderItem.RuleFor(r => r.Quantity)
                        .GreaterThan(0)
                        .WithMessage(Errors.General.ValueTooSmall(nameof(CreateOrderItem.Quantity), 1).Message);
                    orderItem.RuleFor(r => r.TotalPrice)
                        .GreaterThan(0)
                        .WithMessage(Errors.General.ValueTooSmall(nameof(CreateOrderItem.TotalPrice), 1).Message);
                });
                    RuleFor(r => r.Discount).InclusiveBetween(0, 15).WithMessage(Errors.General.ValueOutOfRange(nameof(Discount), 0, 15).Message);
            }
        }
    }
}
