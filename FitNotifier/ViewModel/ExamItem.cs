using FitNotifier.Data.Services.Kos.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitNotifier.ViewModel
{
    public class ExamItem
    {
        public Exam Kos { get; set; }
        public bool Registered { get; set; }
        public bool Avaible => Kos.Occupied < Kos.Capacity;

        public ExamItem(Exam kos)
        {
            Kos = kos;
        }

        public ExamItem()
        {

        }
    }
}
