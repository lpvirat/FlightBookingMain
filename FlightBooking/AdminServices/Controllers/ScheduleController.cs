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
    public class ScheduleController : ControllerBase
    {
        [Authorize]
        [HttpPost]
        [Route("ScheduleFlight")]
        public IActionResult AddFlight([FromBody] Flights flight)
        {
            if (flight == null)
                return BadRequest();
            else
            {
                var db = new AdminDBContext();
                var airlines = db.AirlineaddBlock.ToList();
                var existingAirline = airlines.Where(x => x.AirlineId == flight.FlightId).FirstOrDefault();

                if (existingAirline == null)
                {
                    return Ok(new
                    {
                        success = 0,
                        message = "Airline doesnot exist",
                    });
                }
                else
                {
                    db.Flights.Add(flight);
                    db.SaveChanges();
                    return Ok(new
                    {
                        success = 1,
                        message = flight.FlightName + " added Successfully"
                    });
                }
            }
        }
    }
}
