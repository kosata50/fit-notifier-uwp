using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FitNotifier.Extensions
{
    public class WebViewEx
    {
        public static string GetHTML(DependencyObject obj)
        {
            return (string)obj.GetValue(HTMLProperty);
        }

        public static void SetHTML(DependencyObject obj, string value)
        {
            obj.SetValue(HTMLProperty, value);
        }

        public static readonly DependencyProperty HTMLProperty =
            DependencyProperty.RegisterAttached("HTML", typeof(string), typeof(WebViewEx), new PropertyMetadata(string.Empty, OnHTMLChanged));

        private static string htmlTemplate;

        private static async void OnHTMLChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WebView view = d as WebView;
            view.NavigationStarting += View_NavigationStarting;
            if (view != null)
                view.NavigateToString(await InsertTemplate((string)e.NewValue));
        }

        private static void View_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            args.Cancel = args.Uri != null;     
        }

        private static async Task<string> InsertTemplate(string html)
        {
            if (htmlTemplate == null)
            {
                var file = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync(@"Assets\Classification.html");
                htmlTemplate = await Windows.Storage.FileIO.ReadTextAsync(file);
            }
            return htmlTemplate.Replace("{content}", html);
        }
    }
}
