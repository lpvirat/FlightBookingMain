using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookingServices.Models
{
    public class Coupons
    {
        [Key]
        public string CouponName { get; set; }
        public int Discount { get; set; }
    }
}
