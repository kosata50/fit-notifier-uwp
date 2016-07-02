using FitNotifier.Data.Model;
using FitNotifier.Data.Services;
using FitNotifier.Data.Services.Edux.Entities;
using FitNotifier.Data.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitNotifier.Data.Update
{
    public class CoursesRefresher
    {
        public  CoursesStorage Storage { get; private set; }
        private Settings settings;
        public bool Busy { get; private set; }

        public CoursesRefresher(Settings settings)
        {
            Storage = new CoursesStorage();
            this.settings = settings;
        }

        public async Task<List<CourseInfo>> RefreshCourses()
        {
            Busy = true;
            List<CourseInfo> courses = await Storage.LoadCourses();
            CourseUpdater updater = new CourseUpdater(settings.User);
            await updater.UpdateCourses(courses);

            EduxUpdater edux = new EduxUpdater(settings.User);
            EduxStorage eduxStorage = new EduxStorage();
            ExamsStorage examsStorage = new ExamsStorage();
            foreach (CourseInfo c in courses)
            {
                EduxClassification classification = await eduxStorage.LoadClassification(c.Kos);
                c.EduxChanges = await edux.UpdateClassification(classification);
                await eduxStorage.SaveClassification(classification);

                List<ExamInfo> exams = await examsStorage.LoadExams(c.Kos.Code);
                c.KosChanges = await updater.UpdateExams(exams, c.Kos);
                await examsStorage.SaveExams(exams, c.Kos.Code);
            }
            settings.Entries.LastRefresh = DateTime.Now;

            await Storage.SaveCourses(courses);
            await new SettingsStorage().SaveSettings(settings);
            Busy = false;
            return courses;
        }
    }
}
