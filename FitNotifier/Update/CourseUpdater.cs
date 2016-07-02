using FitNotifier.Data.Services;
using FitNotifier.Data.Services.Kos;
using FitNotifier.Data.Services.Kos.Entities;
using FitNotifier.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitNotifier.Update
{
    public class CourseUpdater
    {
        public KosService Kos { get; private set; }
        public UserCredencials User { get; private set; }

        public CourseUpdater(UserCredencials user)
        {
            User = user;
        }


        public async Task UpdateCourses(List<CourseItem> courses)
        {
            await GetKosService(User);
            if (Kos != null)
            {
                List<Course> newCourses = await Kos.GetCourses(User.Username);
                courses.Clear();
                courses.AddRange(newCourses.Select(c => new CourseItem(c)));
            }
        }

        public async Task<bool> UpdateExams(List<ExamItem> exams, Course course)
        {
            await GetKosService(User);
            if (Kos != null)
            {

                List<Exam> newExams = await Kos.GetExams(course.Code);
                Dictionary<string, ExamRegistration> registrations = (await Kos.GetExamRegistrations(User.Username)).ToDictionary(r => r.ID);
                exams.Clear();
                exams.AddRange(newExams.Select(e => new ExamItem(e)
                {
                    Registered = registrations.ContainsKey(e.ID)
                }));
            }
            return false;
        }

        private async Task GetKosService(UserCredencials user)
        {
            if (Kos == null)
            {
                KosCredencials credencials = await new KosAuthService().RequestCredencials(user);
                if (credencials != null)
                    Kos = new KosService(credencials);
            }
        }
    }
}
