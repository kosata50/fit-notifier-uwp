using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace FitNotifier.Extensions
{
    public class BoolToColorConverter : IValueConverter
    {
        public SolidColorBrush TrueColor { get; set; }
        public SolidColorBrush FalseColor { get; set; }


        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool b = (bool)value;
            return b ? TrueColor : FalseColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
