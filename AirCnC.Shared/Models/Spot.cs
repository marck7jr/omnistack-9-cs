namespace AirCnC.Shared.Models
{
    public class Spot : Model
    {
        private string thumbnail;
        private string company;
        private double price;
        private string techs;
        private User user;

        public string Thumbnail { get => thumbnail; set => Set(ref thumbnail, value); }
        public string Company { get => company; set => Set(ref company, value); }
        public double Price { get => price; set => Set(ref price, value); }
        public string Techs { get => techs; set => Set(ref techs, value); }
        public User User { get => user; set => Set(ref user, value); }
    }
}
