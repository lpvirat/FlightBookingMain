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
    public class AddBlockController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        [Route("AllFlightDetails")]
        public List<AirlineAddBlock> AllFlightDetails()
        {
            using (var db = new AdminDBContext())
            {
                return db.AirlineaddBlock.ToList();
            }
        }


        [Authorize]
        [HttpPost]
        [Route("AddFlight")]
        public int AddFlight([FromBody] AirlineAddBlock data)
        {
            if (string.IsNullOrEmpty(data.AirlineId) || string.IsNullOrEmpty(data.Airlinename))
                return 0;
            else
            {
                using (var db = new AdminDBContext())
                {
                    db.AirlineaddBlock.Add(data);
                    db.SaveChanges();
                    return 1;
                }
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("BlockFlight")]
        public IActionResult BlockFlight(string airline_ID)
        {
            var db = new AdminDBContext();

            if (string.IsNullOrEmpty(airline_ID))
            {
                return NotFound(new
                {
                    success = 0,
                    message = "Please enter valid AirlineId"
                });
            }
            
            else
            {

                var airline = db.AirlineaddBlock.Where(x => x.AirlineId == airline_ID).FirstOrDefault();
                var flight = db.Flights.Where(x => x.FlightId == airline_ID).FirstOrDefault();
                if(airline!= null)
                {
                    db.AirlineaddBlock.Remove(airline);
                    db.Flights.Remove(flight);
                    db.SaveChanges();
                    return Ok(new
                    {
                        success = 1,
                        message = "Airline " + airline_ID + " removed Successfully"

                    });
                }
                else
                {
                    return NotFound(new
                    {
                        success = 0,
                        message = "Please enter valid AirlineId"
                    });
                }
            }
        }
    }
}
