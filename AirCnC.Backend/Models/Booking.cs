using System;

namespace AirCnC.Backend.Models
{
    public class Booking : Model
    {
        public DateTime Date { get; set; }
        public bool Approved { get; set; }
        public User User { get; set; }
        public Spot Spot { get; set; }
    }
}
