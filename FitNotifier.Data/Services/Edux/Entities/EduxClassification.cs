using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitNotifier.Data.Services.Edux.Entities
{
    public class EduxClassification
    {
        public string CourseCode { get; set; }
        public string HTML { get; set; }

        public bool Empty => string.IsNullOrEmpty(HTML);
    }
}
