using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminServices.Models
{
    public class Flights
    {
        public int Id { get; set; }
        public string FlightId { get; set; }
        public string FlightName { get; set; }
        public string FlightLogo { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int ScheduledDays { get; set; }
        public int Bcseats { get; set; }
        public int Nbcseats { get; set; }
        public int Price { get; set; }
        public string MealType { get; set; }
        public string TypeOfTrip { get; set; }
    }
}
