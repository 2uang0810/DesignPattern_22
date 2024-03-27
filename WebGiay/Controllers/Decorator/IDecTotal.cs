using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebGiay.Controllers.Decorator
{
    public interface IDiscountDecorator
    {
        decimal ApplyDiscount(decimal totalPrice);
    }
}