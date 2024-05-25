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
        public List<Product> Products { get; set; }
        public int Discount { get; set; }


        public class Validator : AbstractValidator<CreateOrderRequest>
        {
            public Validator()
            {
                RuleFor(r => r.CustomerId).NotEmpty().WithMessage(Errors.General.ValueIsEmpty(nameof(CustomerId)).Message);
                RuleForEach(r => r.Products).ChildRules(product =>
                {
                    product.RuleFor(r => r.Id)
                        .NotEmpty();
                });
                    RuleFor(r => r.Discount).InclusiveBetween(0, 15).WithMessage(Errors.General.ValueOutOfRange(nameof(Discount), 0, 15).Message);
            }
        }
    }
}
