using AdminServices.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponsController : ControllerBase
    {
       
        [Authorize]
        [HttpPost]
        [Route("AddCoupon")]
        public IActionResult AddCoupon([FromBody] Coupons data)
        {
            if (string.IsNullOrEmpty(data.CouponName) || data.CouponName == "" || data.Discount <= 0)
                return Ok(new
                {
                    success = 0,
                    Message = "Please enter Coupon and discount properly",
                });

            else
            {
                using (var db = new AdminDBContext())
                {
                    var couponcode = db.Coupon.Where(x => x.CouponName == data.CouponName).FirstOrDefault();
                    if (couponcode!=null)
                    {
                        return Ok(new
                        {
                            success = 0,
                            Message = "Coupon already exists",
                        });
                    }
                    else
                    {
                        db.Coupon.Add(data);
                        db.SaveChanges();
                        return Ok(new
                        {
                            success = 1,
                            message = "Coupon added Successfully"

                        });
                    }
                   
                }
            }
        }
    }
}
