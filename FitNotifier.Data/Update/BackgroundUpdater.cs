﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace FitNotifier.Data.Update
{
    public class BackgroundUpdater
    {
        public async Task Register()
        {
            TimeTrigger trigger = new TimeTrigger(15, false);
            SystemCondition condition = new SystemCondition(SystemConditionType.InternetAvailable);
            await BackgroundExecutionManager.RequestAccessAsync();

            BackgroundTaskRegistration reg = RegisterBackgroundTask("FitNotifier.Tasks.RefreshCoursesTask",
                "RefreshCourses", trigger, condition);
        }

        public static BackgroundTaskRegistration RegisterBackgroundTask(string taskEntryPoint,
                                                                string taskName,
                                                                IBackgroundTrigger trigger,
                                                                IBackgroundCondition condition)
        {
            //
            // Check for existing registrations of this background task.
            //

            foreach (var cur in BackgroundTaskRegistration.AllTasks)
            {

                if (cur.Value.Name == taskName)
                {
                    // 
                    // The task is already registered.
                    // 

                    return (BackgroundTaskRegistration)(cur.Value);
                }
            }

            //
            // Register the background task.
            //

            var builder = new BackgroundTaskBuilder();

            builder.Name = taskName;
            builder.TaskEntryPoint = taskEntryPoint;
            builder.SetTrigger(trigger);

            if (condition != null)
            {

                builder.AddCondition(condition);
            }

            BackgroundTaskRegistration task = builder.Register();

            return task;
        }
    }
}