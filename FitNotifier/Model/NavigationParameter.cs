using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitNotifier.Model
{
    public class NavigationParameter
    {
        public string Title { get; private set; }
        public Object Parameter { get; private set; }

        public NavigationParameter(string title, object parameter = null)
        {
            Title = title;
            Parameter = parameter;
        }
    }
}
