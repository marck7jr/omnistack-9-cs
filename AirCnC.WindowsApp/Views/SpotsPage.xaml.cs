using AirCnC.Shared.Helpers;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=234238

namespace AirCnC.WindowsApp.Views
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class SpotsPage : Page
    {
        public string[] Techs { get; set; }

        public SpotsPage()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is string techs)
            {
                Techs = techs.GetWords().ToArray();
            }
        }
    }
}
