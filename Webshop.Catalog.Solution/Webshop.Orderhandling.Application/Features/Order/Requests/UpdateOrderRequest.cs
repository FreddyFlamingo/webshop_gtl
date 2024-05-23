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
        public int OrderId { get; set; }
        public List<UpdateOrderItem> OrderItems { get; set; }

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
                RuleFor(r => r.OrderId)
                    .GreaterThan(0)
                    .WithMessage(Errors.General.ValueTooSmall(nameof(OrderId), 0).Message);
                RuleForEach(r => r.OrderItems)
                    .SetValidator(new UpdateOrderItemValidator());
            }

            public class UpdateOrderItemValidator : AbstractValidator<UpdateOrderItem>
            {
                public UpdateOrderItemValidator()
                {
                    RuleFor(r => r.ProductId)
                        .GreaterThan(0)
                        .WithMessage(Errors.General.ValueTooSmall(nameof(UpdateOrderItem.ProductId), 0).Message);
                    RuleFor(r => r.Quantity)
                        .GreaterThan(0)
                        .WithMessage(Errors.General.ValueTooSmall(nameof(UpdateOrderItem.Quantity), 0).Message);
                    RuleFor(r => r.UnitPrice)
                        .GreaterThan(0)
                        .WithMessage(Errors.General.ValueTooSmall(nameof(UpdateOrderItem.UnitPrice), 0).Message);
                }
            }
        }
    }
}
