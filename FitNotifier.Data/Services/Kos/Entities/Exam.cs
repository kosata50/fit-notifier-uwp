using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitNotifier.Data.Services.Kos.Entities
{
    public class Exam
    {
        public string ID { get; set; }
        public string CourseCode { get; set; }
        public int Capacity { get; set; }
        public int Occupied { get; set; }
        public string Room { get; set; }
        public string Note { get; set; }
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public ExamType Type { get; set; }
    }

    public enum ExamType
    {
        FinalExam,
        Assessment
    }
}
