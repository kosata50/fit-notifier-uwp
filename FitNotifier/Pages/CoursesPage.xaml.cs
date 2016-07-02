using FitNotifier.Data.Model;
using FitNotifier.Data.Services;
using FitNotifier.Data.Services.Kos;
using FitNotifier.Data.Storage;
using FitNotifier.Data.Update;
using FitNotifier.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
    public sealed partial class CoursesPage : Page
    {
        CoursesViewModel model;
        CoursesRefresher refresher;

        public CoursesPage()
        {
            this.InitializeComponent();
        }


        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Settings settings = await new SettingsStorage().LoadSettings();
            DataContext = model = new CoursesViewModel(!settings.LoggedIn);
            if (settings.LoggedIn)
            {
                refresher = new CoursesRefresher(settings);
                //await refresher.Storage.DeleteCourses();//DEBUG
                await LoadCourses();
                if (model.Courses.Count == 0)
                    await RefreshCourses();
            }
            await new BackgroundUpdater().Register();
        }

        private async Task LoadCourses()
        {
            IList<CourseItem> courses = (await refresher.Storage.LoadCourses()).Select(c => new CourseItem(c)).ToList();
            model.UpdateCouses(courses);
        }

        private async Task RefreshCourses()
        {
            if (!refresher.Busy)
            {
                progressBar.IsIndeterminate = true;
                try
                {
                    List<CourseItem> courses = (await refresher.RefreshCourses()).Select(c => new CourseItem(c)).ToList();
                    model.UpdateCouses(courses);
                }
                catch
                {
                    MessageDialog dialog = new MessageDialog("Zkontrolujete stav připojení.", "Předměty se nepodařilo obnovit");
                    await dialog.ShowAsync();
                }
                progressBar.IsIndeterminate = false;
            }
        }

        private async void Refresh_Click(object sender, RoutedEventArgs e)
        {
            await RefreshCourses();
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingsPage), new NavigationParameter("Nastavení"));
        }

        private void Course_Tapped(object sender, TappedRoutedEventArgs e)
        {
            CourseItem item = (sender as Grid).DataContext as CourseItem;
            Frame.Navigate(typeof(CourseDetailPage), new NavigationParameter(item.Kos.Code, item));
        }
    }
}
