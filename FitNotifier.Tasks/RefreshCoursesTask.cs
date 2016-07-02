using FitNotifier.Data.Model;
using FitNotifier.Data.Storage;
using FitNotifier.Data.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace FitNotifier.Tasks
{
    public sealed class RefreshCoursesTask : IBackgroundTask
    {
        BackgroundTaskDeferral deff;
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            deff = taskInstance.GetDeferral();

            try
            {
                Settings settings = await new SettingsStorage().LoadSettings();
                CoursesRefresher refresher = new CoursesRefresher(settings);
                await refresher.RefreshCourses();
            }
            catch
            {
            }

            deff.Complete();
        }
    }
}
