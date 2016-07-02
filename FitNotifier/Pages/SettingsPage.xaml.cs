using FitNotifier.Data.Services;
using FitNotifier.Data.Update;
using FitNotifier.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace FitNotifier.Pages
{
    public sealed partial class SettingsPage : Page
    {
        SettingsViewModel model;
        UserUpdater updater;

        public SettingsPage()
        {
            this.InitializeComponent();
            updater = new UserUpdater();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            DataContext = model = new SettingsViewModel(await updater.GetSettings());
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(model.Username) && !string.IsNullOrEmpty(model.Password))
            {
                UserCredencials user = new UserCredencials(model.Username, model.Password);
                model.SetLoginProgress(true);
                try
                {
                    if (await updater.Login(user, model.Settings))
                    {
                        if (Frame.CanGoBack)
                            Frame.GoBack();
                    }
                    else
                    {
                        model.SetLoginProgress(false);
                        MessageDialog dialog = new MessageDialog("Neplatná kombinace jména a hesla.", "Přihlášení se nezdařilo");
                        await dialog.ShowAsync();
                    }
                }
                catch
                {
                    model.SetLoginProgress(false);
                    MessageDialog dialog = new MessageDialog("Zkontrolujte připojení k internetu a fakultním službám.",
                        "Nebylo možné ověřit účet");
                    await dialog.ShowAsync();
                }
            }
        }

        private async void Logout_Click(object sender, RoutedEventArgs e)
        {
            await updater.Logout(model.Settings);
            model.Logout();
        }
    }
}
