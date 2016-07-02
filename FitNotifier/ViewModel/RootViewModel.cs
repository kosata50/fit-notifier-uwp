using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitNotifier.ViewModel
{
    class RootViewModel : ViewModelBase
    {
        private string title;

        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        private bool back;

        public bool CanGoBack
        {
            get { return back; }
            set
            {
                back = value;
                OnPropertyChanged(nameof(CanGoBack));
            }
        }
    }
}
