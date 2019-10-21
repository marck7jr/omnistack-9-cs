using AirCnC.Shared.Models;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

// O modelo de item de Caixa de Diálogo de Conteúdo está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace AirCnC.WindowsApp.UI.Controls
{
    public sealed partial class BookingDialog : ContentDialog
    {
        public Booking Booking { get; set; }

        public BookingDialog()
        {
            this.InitializeComponent();
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            using (var client = new HttpClient())
            {
                var userGuid = JsonConvert.DeserializeObject<User>(ApplicationData.Current.LocalSettings.Values["userGuid"] as string).Guid;

                client.BaseAddress = new Uri("https://localhost:5001");
                client.DefaultRequestHeaders.Add(nameof(userGuid), userGuid.ToString());

                var content = new StringContent(JsonConvert.SerializeObject(new Booking()
                {
                    Date = CalendarDatePicker.Date.Value.UtcDateTime,
                }), Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"api/spots/{SpotView.Spot.Guid}/bookings", content);

                if (response.IsSuccessStatusCode)
                {

                }
            }
        }
    }
}
