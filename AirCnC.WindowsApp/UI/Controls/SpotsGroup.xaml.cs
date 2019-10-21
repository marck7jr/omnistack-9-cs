using AirCnC.Shared.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Web;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

// O modelo de item de Controle de Usuário está documentado em https://go.microsoft.com/fwlink/?LinkId=234236

namespace AirCnC.WindowsApp.UI.Controls
{
    public sealed partial class SpotsGroup : UserControl
    {
        public string Tech { get => DataContext as string; }
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

        private async void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var dialog = new BookingDialog();

            dialog.DataContext = e.ClickedItem as Spot;

            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
            {
                await new MessageDialog("Reserva solicitada com sucesso.").ShowAsync();
            }
        }
    }
}
