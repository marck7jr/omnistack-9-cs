using AirCnC.Shared.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace AirCnC.WindowsApp.Helpers
{
    public static class BookingHubHelper
    {
        public static HubConnection Connection { get; set; }
        public static ObservableCollection<Booking> Bookings { get; set; } = new ObservableCollection<Booking>();

        public static async Task InitializeAsync(Guid userGuid)
        {
            Connection = new HubConnectionBuilder()
                .WithUrl($"https://localhost:5001/bookings?user={userGuid}")
                .WithAutomaticReconnect()
                .Build();

            Connection.On<string, string>("ReceiveBooking", (to, message) =>
            {
                Console.WriteLine($"to: {to}");
                Console.WriteLine($"message: {message}");

                Bookings.Add(JsonConvert.DeserializeObject<Booking>(message));
            });

            Bookings.CollectionChanged += async (s, e) =>
            {
                foreach (Booking item in e.NewItems)
                {
                    var message = item.IsApproved ?
                        $"Sua reserva em {item.Spot.Company} no dia {item.Date} foi aprovada." :
                        $"Sua reserva em {item.Spot.Company} no dia {item.Date} foi recusada.";

                    await new MessageDialog(message).ShowAsync();
                }
            };

            await Connection.StartAsync();
        }
    }
}