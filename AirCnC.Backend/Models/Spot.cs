namespace AirCnC.Backend.Models
{
    public class Spot : Model
    {
        public string Thumbnail { get; set; }
        public string Company { get; set; }
        public double Price { get; set; }
        public string Techs { get; set; }
        public User User { get; set; }
    }
}
