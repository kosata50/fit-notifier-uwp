using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitNotifier.Data.Services.Kos.Entities
{
    public class Course
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Semester { get; set; }
        public string SemesterCode { get; set; }
        public bool Assessment { get; set; }
        public bool Completed { get; set; }
    }
}
