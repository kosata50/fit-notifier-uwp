using FitNotifier.Data.Services.Edux.Entities;
using FitNotifier.Data.Storage;
using FitNotifier.ViewModel;
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
using Windows.UI.Xaml.Navigation;


namespace FitNotifier.Pages
{
    public sealed partial class CourseDetailPage : Page
    {
        CourseDetailViewModel model;

        public CourseDetailPage()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            CourseItem item = (e.Parameter as NavigationParameter).Parameter as CourseItem;
            EduxClassification edux = await new EduxStorage().LoadClassification(item.Kos);
            List<ExamItem> exams = (await new ExamsStorage().LoadExams(item.Kos.Code)).Select(ex => new ExamItem(ex)).ToList();
            DataContext = model = new CourseDetailViewModel(item.Kos, edux, exams);
        }
    }
}
