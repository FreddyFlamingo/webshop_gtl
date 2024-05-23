using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Webshop.Domain.Common;

namespace Webshop.Orderhandling.Application.Features.Order.Requests
{
    public class CreateOrderRequest
    {
        public string CustomerId { get; set; }
        public List<CreateOrderItem> OrderItems { get; set; }

        public class CreateOrderItem
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
        }

        public class Validator : AbstractValidator<CreateOrderRequest>
        {
            public Validator()
            {
                RuleFor(r => r.CustomerId)
                    .NotEmpty()
                    .WithMessage(Errors.General.ValueIsRequired(nameof(CustomerId)).Code + " (" + Errors.General.ValueIsRequired(nameof(CustomerId)).Message + ")");
                RuleForEach(r => r.OrderItems)
                    .SetValidator(new CreateOrderItemValidator());
            }

            public class CreateOrderItemValidator : AbstractValidator<CreateOrderItem>
            {
                public CreateOrderItemValidator()
                {
                    RuleFor(r => r.ProductId)
                        .GreaterThan(0)
                        .WithMessage(Errors.General.ValueTooSmall(nameof(CreateOrderItem.ProductId), 0).Message);
                    RuleFor(r => r.Quantity)
                        .GreaterThan(0)
                        .WithMessage(Errors.General.ValueTooSmall(nameof(CreateOrderItem.Quantity), 0).Message);
                    RuleFor(r => r.UnitPrice)
                        .GreaterThan(0)
                        .WithMessage(Errors.General.ValueTooSmall(nameof(CreateOrderItem.UnitPrice), 0).Message);
                }
            }
        }
    }
}
