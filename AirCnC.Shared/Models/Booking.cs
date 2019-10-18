using System;

namespace AirCnC.Shared.Models
{
    public class Booking : Model
    {
        private DateTime date;
        private bool approved;
        private User user;
        private Spot spot;

        public DateTime Date { get => date; set => Set(ref date, value); }
        public bool Approved { get => approved; set => Set(ref approved, value); }
        public User User { get => user; set => Set(ref user, value); }
        public Spot Spot { get => spot; set => Set(ref spot, value); }
    }
}
