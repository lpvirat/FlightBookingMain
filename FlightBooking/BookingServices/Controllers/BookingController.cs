using AdminServices.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Userservices.Models;

namespace BookingServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        [Route("GetAllFlights")]
        public IList<Flights> GetAllFlights()
        {
            using (var db = new AdminDBContext())
            {
                return db.Flights.ToList();
            }
        }

        [Authorize]
        [HttpPost]
        [Route("GetFlightsBySearch")]
        public IList<Flights> GetFlightsBySearch([FromBody] Flights obj)
        {
            var db = new AdminDBContext();
            var flights = db.Flights.Where(t => t.Source == obj.Source && t.Destination == obj.Destination && t.TypeOfTrip == obj.TypeOfTrip).ToList();

            return flights;
        }

        [Authorize]
        [HttpPost]
        [Route("FlightTicketBooking")]
        public IActionResult InsertTicketBooking([FromBody] TicketBooking ticket)
        {
            string pnr = string.Empty;
            if (string.IsNullOrEmpty(ticket.AirlineId) || string.IsNullOrEmpty(ticket.UserName) || string.IsNullOrEmpty(ticket.Name)
                                                       || string.IsNullOrEmpty(ticket.Gender) || string.IsNullOrEmpty(ticket.SeatNumbers))
                return NotFound(new
                {
                    success = 0,
                    message = "Please enter valid details"
                });
            else
            {
                var db = new UserDBContext();
                var admindb = new AdminDBContext();
                var userDetails = db.UserCredentials.Where(x => x.UserName == ticket.UserName).FirstOrDefault();
                var flight = admindb.Flights.Where(x => x.FlightId == ticket.AirlineId).FirstOrDefault();
                if (flight != null)
                {
                    ticket.BoardingTime = flight.StartTime.ToString();
                    ticket.Source = flight.Source;
                    ticket.Destination = flight.Destination;
                    ticket.MealType = flight.MealType;
                    ticket.EmailId = userDetails.Email;

                    Random random = new Random();
                    //int seatNo = random.Next(00,99);
                    int pnrID = random.Next(000000, 999999);
                    pnr = "PNR" + pnrID;
                    ticket.Pnr = pnr;
                    ticket.IsCancelled = 0;

                    db.TicketBooking.Add(ticket);
                    db.SaveChanges();
                    return Ok(new
                    {
                        success = 1,
                        message = "Ticket booked Successfully",
                        pnr = pnr
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

        [Authorize]
        [HttpGet]
        [Route("SearchTicket")]
        public IList<TicketBooking> TicketSearch(string emailId)
        {
            var db = new UserDBContext();
            var tickets = new List<TicketBooking>();

            if (emailId == null)
                return tickets;
            else
            {
                tickets = db.TicketBooking.Where(t => t.EmailId == emailId).ToList();

                return tickets;
            }
        }

        [Authorize]
        [HttpPut]
        [Route("CancelTicket")]
        public IActionResult CancelTicket([FromBody] TicketBooking cancelTicket)
        {
            if (string.IsNullOrEmpty(cancelTicket.Pnr) && string.IsNullOrEmpty(cancelTicket.EmailId))
                return Ok(new
                {
                    success = 0,
                    Message = "Please enter PNR number and EmailId properly",
                });
            else
            {
                var db = new UserDBContext();
                var ticket = db.TicketBooking.Where(t => t.Pnr == cancelTicket.Pnr && t.EmailId == cancelTicket.EmailId).FirstOrDefault();
           

                if (ticket != null )
                {
                    if (ticket.IsCancelled == 1)
                    {
                        return Ok(new
                        {
                            success = 0,
                            Message = "Ticket with this PNR is already cancelled",
                        });
                    }
                    ticket.IsCancelled = 1;
                    db.SaveChanges();

                    return Ok(new
                    {
                        success = 1,
                        Message = "Ticket is cancelled",
                        pnr = ticket.Pnr
                    });
                }
                else
                {
                    return Ok(new
                    {
                        success = 0,
                        Message = "Please enter valid PNR number and emailId",
                    });
                }
            }
        }
    }
}
