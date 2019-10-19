using AirCnC.Shared.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Web;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

// O modelo de item de Controle de Usuário está documentado em https://go.microsoft.com/fwlink/?LinkId=234236

namespace AirCnC.WindowsApp.UI.Controls
{
    public sealed partial class SpotsGroup : UserControl
    {
        public string Tech { get; set; }
        public ObservableCollection<Spot> Spots { get; set; } = new ObservableCollection<Spot>();
        public SpotsGroup()
        {
            this.InitializeComponent();
            this.DataContextChanged += (s, e) => Bindings.Update();
            this.Loaded += async (s, e) =>
            {
                using (var client = new HttpClient())
                {
                    Debug.WriteLine(Tech);

                    client.BaseAddress = new Uri("https://localhost:5001");

                    var response = await client.GetAsync($"/api/spots?techs={HttpUtility.UrlEncode(Tech)}");

                    if (response.IsSuccessStatusCode)
                    {
                        JsonConvert.DeserializeObject<List<Spot>>(await response.Content.ReadAsStringAsync())
                            .ForEach(spot => Spots.Add(spot));
                    }
                }
            };
        }

        public static BitmapImage GetBitmapImage(string url)
        {
            return new BitmapImage()
            {
                UriSource = new Uri($"https://localhost:5001{url}")
            };
        }

        public static string GetPriceString(double price)
        {
            return price > 0 ? $"R$ {price}" : "Gratuito";
        }
    }
}
