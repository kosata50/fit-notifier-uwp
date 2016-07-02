using FitNotifier.Data.Model;
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
        public Exam Kos => info.Kos;
        public bool Registered => info.Registered;
        public bool Avaible => Kos.Occupied < Kos.Capacity;
        private ExamInfo info;

        public ExamItem(ExamInfo info)
        {
            this.info = info;
        }
    }
}
