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

            await Task.Delay(500);

            deff.Complete();
        }
    }
}
