using AdminServices.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
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
        public IActionResult BlockFlight(string airlineId)
        {
            

            if (string.IsNullOrEmpty(airlineId))
            {
                return Ok(new
                {
                    success = 0,
                    message = "Please enter valid AirlineId"
                });
            }
           
            
            else
            {
                using (var db = new AdminDBContext())
                {

                    var airline = db.AirlineaddBlock.Where(x => x.AirlineId == airlineId).FirstOrDefault();
                    var flight = db.Flights.Where(x => x.FlightId == airlineId).FirstOrDefault();

                    if(airline!= null)
                    {
                        db.AirlineaddBlock.Remove(airline);

                        if (flight != null)
                        {
                            db.Flights.Remove(flight);
                        }

                        db.SaveChanges();

                        var factory = new ConnectionFactory
                        {
                            Uri = new Uri("amqp://guest:guest@localhost:5672")
                        };
                        var connection = factory.CreateConnection();
                        var channel = connection.CreateModel();
                        Producer.Publish(channel,airline.AirlineId);

                        return Ok(new
                        {
                            success = 1,
                            message = "Airline " + airlineId + " removed Successfully"

                        });

                       
                    }
                    else
                    {
                        return Ok(new
                        {
                            success = 0,
                            message = "Please enter valid AirlineId"
                        });
                    }
                    

                }
              
            }

        }
    }
}
