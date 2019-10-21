using AirCnC.Shared.Models;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

// O modelo de item de Controle de Usuário está documentado em https://go.microsoft.com/fwlink/?LinkId=234236

namespace AirCnC.WindowsApp.UI.Controls
{
    public sealed partial class SpotView : UserControl
    {
        public Spot Spot { get => DataContext as Spot; }

        public SpotView()
        {
            this.InitializeComponent();
            this.DataContextChanged += (s, e) => Bindings.Update();
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
