using FitNotifier.Data.Services.Kos.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitNotifier.ViewModel
{
    public class ExamsGroup : ObservableCollection<ExamItem>
    {
        public ExamType Key { get; set; }

        public ExamsGroup(ExamType key, IEnumerable<ExamItem> exams)
        {
            Key = key;
            foreach (ExamItem e in exams)
                Add(e);
        }

        public ExamsGroup()
        {
        }
    }
}
