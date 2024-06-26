﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Webshop.Catalog.Domain.AggregateRoots;
using Webshop.Domain.Common;

namespace Webshop.Orderhandling.Domain.AggregateRoots
{
    public class Order : AggregateRoot
    {
        public DateTime OrderDate { get; set; }
        public string CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Discount { get; set; }
        public List<Product> Products { get; set; }

        public Order()
        {
            Products = new List<Product>();
        }

        public Result ApplyDiscount(decimal discount)
        {
            if (discount < 0 || discount > 15)
            {
                //throw new ArgumentException("Discount must be between 0% and 15%");
                return Result.Fail(Errors.General.ValueOutOfRange("Discount", 0, 15));
            }
            Discount = discount;
            CalculateTotalAmount();
            return Result.Ok();
        }

        private void CalculateTotalAmount()
        {
            decimal total = 0;
            foreach (var item in Products)
            {
                total += item.Price;
            }
            TotalAmount = total - (total * (Discount / 100));
        }


    }
}
