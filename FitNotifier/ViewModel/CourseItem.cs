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
        public Course Kos { get; set; }
        public bool EduxChanges { get; set; }
        public bool KosChanges { get; set; }

        public CourseItem(Course course)
        {
            Kos = course;
        }

        public CourseItem()
        {

        }


        public void Update(Course kos)
        {
            Kos = kos;
            OnPropertyChanged(nameof(Kos));
        }
    }
}
