using FitNotifier.Data.Services;
using FitNotifier.Data.Services.Edux.Entities;
using FitNotifier.Storage;
using FitNotifier.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitNotifier.Update
{
    public class CoursesRefresher
    {
        public  CoursesStorage Storage { get; private set; }
        private UserCredencials user;
        public bool Busy { get; private set; }

        public CoursesRefresher(UserCredencials user)
        {
            Storage = new CoursesStorage();
            this.user = user;
        }

        public async Task<List<CourseItem>> RefreshCourses()
        {
            Busy = true;
            List<CourseItem> courses = await Storage.LoadCourses();
            CourseUpdater updater = new CourseUpdater(user);
            await updater.UpdateCourses(courses);

            EduxUpdater edux = new EduxUpdater(user);
            EduxStorage eduxStorage = new EduxStorage();
            ExamsStorage examsStorage = new ExamsStorage();
            foreach (CourseItem c in courses)
            {
                EduxClassification classification = await eduxStorage.LoadClassification(c.Kos);
                c.EduxChanges = await edux.UpdateClassification(classification);
                await eduxStorage.SaveClassification(classification);

                List<ExamItem> exams = await examsStorage.LoadExams(c.Kos.Code);
                c.KosChanges = await updater.UpdateExams(exams, c.Kos);
                await examsStorage.SaveExams(exams, c.Kos.Code);
            }

            await Storage.SaveCourses(courses);
            Busy = false;
            return courses;
        }
    }
}
