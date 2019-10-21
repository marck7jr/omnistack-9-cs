using AirCnC.Shared.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

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
