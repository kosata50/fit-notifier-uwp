using FitNotifier.Data.Services;
using FitNotifier.Data.Services.Kos;
using FitNotifier.Data.Services.Kos.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitNotifier.ViewModel
{
    public class CoursesViewModel
    {
        public ObservableCollection<CourseItem> Courses { get; private set; }
        public bool LoginPrompt { get; private set; }

        public CoursesViewModel(bool loginPrompt)
        {
            Courses = new ObservableCollection<CourseItem>();
            LoginPrompt = loginPrompt;
        }

        public void UpdateCouses(IList<CourseItem> courses)
        {
            IEnumerable<CourseItem> add = courses.Except(Courses).ToList();
            foreach (CourseItem c in add)
                Courses.Add(c);

            IEnumerable<CourseItem> remove = Courses.Except(courses).ToList();
            foreach (CourseItem c in remove)
                Courses.Remove(c);
        }
    }
}
