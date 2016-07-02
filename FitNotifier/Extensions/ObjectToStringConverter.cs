using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace FitNotifier.Extensions
{
    public class ObjectToStringConverter : IValueConverter
    {
        public PairCollection Pairs { get; set; }

        public ObjectToStringConverter()
        {
            Pairs = new PairCollection();
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            ObjectStringPair pair = Pairs.SingleOrDefault(p => p.Key == value || Enum.ToObject(value.GetType(), p.Key).Equals(value));
            return pair?.Text;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }

    public class ObjectStringPair
    {
        public object Key { get; set; }
        public string Text { get; set; }
    }

    public class PairCollection : List<ObjectStringPair>
    {

    }
}
