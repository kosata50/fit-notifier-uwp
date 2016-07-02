using FitNotifier.Data.Services.Edux.Entities;
using FitNotifier.Data.Services.Kos.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitNotifier.ViewModel
{
    public class CourseDetailViewModel
    {
        public Course Kos { get; private set; }
        public EduxClassification Edux { get; private set; }
        public ObservableCollection<ExamsGroup> Exams { get; private set; }
        public bool NoExams => Exams.Count == 0;

        public CourseDetailViewModel(Course kos, EduxClassification edux, List<ExamItem> exams)
        {
            Kos = kos;
            Edux = edux;
            Exams = new ObservableCollection<ExamsGroup>();
            SortExams(exams);
        }

        private void SortExams(List<ExamItem> exams)
        {
            DateTime today = DateTime.Today;
            var groups = from e in exams
                         where e.Kos.Start >= today
                         orderby e.Kos.Start
                         group e by e.Kos.Type into g
                         orderby g.Key
                         select g;

            foreach (var g in groups)
                Exams.Add(new ExamsGroup(g.Key, g));
        }
    }
}
