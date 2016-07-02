using FitNotifier.Data.Services.Kos.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitNotifier.Data.Model
{
    public class CourseInfo
    {
        public Course Kos { get; set; }
        public bool EduxChanges { get; set; }
        public bool KosChanges { get; set; }
    }
}
