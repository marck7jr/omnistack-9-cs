namespace AirCnC.Shared.Models
{
    public class User : Model
    {
        private string email;

        public string Email { get => email; set => Set(ref email, value); }
    }
}