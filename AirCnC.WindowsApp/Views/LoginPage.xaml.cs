using AirCnC.Shared.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace AirCnC.WindowsApp.Views
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        private User User { get; set; } = new User();

        public LoginPage()
        {
            this.InitializeComponent();
        }

        private async void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:5001");

                var content = new StringContent(JsonConvert.SerializeObject(User), Encoding.UTF8, "application/json");

                var response = await client.PostAsync("/api/users", content);
                if (response.IsSuccessStatusCode)
                {
                    User = JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync());
                    ApplicationData.Current.LocalSettings.Values["userGuid"] = JsonConvert.SerializeObject(User);

                    App.UseBookingHub(User.Guid);

                    Frame.Navigate(typeof(SpotsPage), TechsTextBox.Text);
                }
            }
        }
    }
}
