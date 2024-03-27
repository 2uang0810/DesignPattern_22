using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebGiay.Controllers.Decorator
{
    public class DecTotal : IDiscountDecorator
    {
       
            private readonly decimal _discountRate;

            public DecTotal(decimal discountRate)
            {
                _discountRate = discountRate;
            }

            public decimal ApplyDiscount(decimal totalPrice)
            {
                return totalPrice - (totalPrice * _discountRate / 100);
            }
        
    }
}