using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdminServices.Models
{
    public class AirlineAddBlock
    {
        [Key]
        public int Id { get; set; }
        public string AirlineId { get; set; }
        public string Airlinename { get; set; }
    }
}
