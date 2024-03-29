﻿using AirCnC.Shared.Models;
using AirCnC.WindowsApp.Helpers;
using AirCnC.WindowsApp.Views;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace AirCnC.WindowsApp
{
    /// <resumo>
    ///Fornece o comportamento específico do aplicativo para complementar a classe Application padrão.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Inicializa o objeto singleton do aplicativo. Essa é a primeira linha do código criado
        /// executado e, por isso, é o equivalente lógico de main() ou WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invocado quando o aplicativo é iniciado normalmente pelo usuário final. Outros pontos de entrada
        /// serão usados, por exemplo, quando o aplicativo for iniciado para abrir um arquivo específico.
        /// </summary>
        /// <param name="e">Detalhes sobre a solicitação e o processo de inicialização.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            if (!(Window.Current.Content is Frame rootFrame))
            {
                AddExtendedView();

                // Crie um Quadro para atuar como o contexto de navegação e navegue para a primeira página
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Carregue o estado do aplicativo suspenso anteriormente
                }

                // Coloque o quadro na Janela atual
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // Quando a pilha de navegação não for restaurada, navegar para a primeira página,
                    // configurando a nova página passando as informações necessárias como um parâmetro
                    // de navegação
                    rootFrame.Navigate(typeof(LoginPage));
                }
                // Verifique se a janela atual está ativa
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// Chamado quando ocorre uma falha na Navegação para uma determinada página
        /// </summary>
        /// <param name="sender">O Quadro com navegação com falha</param>
        /// <param name="e">Detalhes sobre a falha na navegação</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invocado quando a execução do aplicativo é suspensa. O estado do aplicativo é salvo
        /// sem saber se o aplicativo será encerrado ou retomado com o conteúdo
        /// da memória ainda intacto.
        /// </summary>
        /// <param name="sender">A origem da solicitação de suspensão.</param>
        /// <param name="e">Detalhes sobre a solicitação de suspensão.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Salvar o estado do aplicativo e parar qualquer atividade em segundo plano
            deferral.Complete();
        }

        private static void AddExtendedView()
        {
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;

            var accentBaseColor = (Color)Current.Resources["AirCnCAccentBaseColor"];
            var accentAltColor = (Color)Current.Resources["AirCnCAccentAltColor"];

            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonForegroundColor = accentBaseColor;

            titleBar.ButtonHoverBackgroundColor = accentBaseColor;
            titleBar.ButtonHoverForegroundColor = Colors.White;

            titleBar.ButtonPressedBackgroundColor = accentAltColor;
            titleBar.ButtonPressedForegroundColor = Colors.White;
        }

        public static async void UseBookingHub(Guid userGuid)
        {
            try
            {
                await BookingHubHelper.InitializeAsync(userGuid);

                BookingHubHelper.Connection.On<string>("ReceiveBooking", async (booking) =>
                {
                    await Window.Current.Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
                    {
                        BookingHubHelper.Bookings.Add(JsonConvert.DeserializeObject<Booking>(booking));
                    });
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

        }
    }
}