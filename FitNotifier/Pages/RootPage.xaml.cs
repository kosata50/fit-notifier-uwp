using FitNotifier.Pages;
using FitNotifier.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace FitNotifier.Pages
{
    public sealed partial class RootPage : Page
    {
        RootViewModel model;

        public RootPage()
        {
            this.InitializeComponent();
            DataContext = model = new RootViewModel();
            contentFrame.Navigated += ContentFrame_Navigated;

            ChangeBar();
            HandleBackButton();
        }


        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (e.Parameter is NavigationParameter)
            {
                model.Title = (e.Parameter as NavigationParameter).Title;
            }
            else
            {
                model.Title = null;
            }
            model.CanGoBack = contentFrame.CanGoBack;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            contentFrame.GoBack();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            contentFrame.Navigate(typeof(CoursesPage));
        }


        private void ChangeBar()
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusbar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
                statusbar.BackgroundOpacity = 1;
                statusbar.BackgroundColor = Color.FromArgb(0, 7, 50, 166);
                statusbar.ForegroundColor = Colors.White;
            }
        }

        private void HandleBackButton()
        {
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += (s, e) => 
            {
                if (contentFrame.CanGoBack && !e.Handled)
                {
                    e.Handled = true;
                    contentFrame.GoBack();
                }
            };
        }
    }
}
