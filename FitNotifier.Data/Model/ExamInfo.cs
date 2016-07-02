using FitNotifier.Data.Services.Kos.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitNotifier.Data.Model
{
    public class ExamInfo
    {
        public Exam Kos { get; set; }
        public bool Registered { get; set; }
    }
}
