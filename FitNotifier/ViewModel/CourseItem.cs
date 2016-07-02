using FitNotifier.Data.Model;
using FitNotifier.Data.Services.Kos.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitNotifier.ViewModel
{
    public class CourseItem : ViewModelBase
    {
        public Course Kos => Info.Kos;
        public CourseInfo Info { get; private set; }

        public CourseItem(CourseInfo info)
        {
            Info = info;
        }

        public void Update(Course kos)
        {
            Info.Kos = kos;
            OnPropertyChanged(nameof(Kos));
        }
    }
}
