using FitNotifier.Data.Services;
using FitNotifier.Data.Services.Kos;
using FitNotifier.Data.Model;
using FitNotifier.Data.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitNotifier.Data.Update
{
    public class UserUpdater
    {
        private SettingsStorage storage;

        public UserUpdater()
        {
            storage = new SettingsStorage();
        }

        public async Task<Settings> GetSettings()
        {
            return await storage.LoadSettings();
        }

        public async Task Logout(Settings settings)
        {
            await new BackgroundUpdater().Unregister();
            await storage.DeleteSettings(settings);
            await new CoursesStorage().DeleteCourses();
            await new EduxStorage().DeleteAllClassifications();
            await new ExamsStorage().DeleteAllExams();
            new KosAuthService().Logout();
        }

        public async Task<bool> Login(UserCredencials user, Settings settings)
        {
            if (await new KosAuthService().RequestCredencials(user) != null)
            {
                settings.User = user;
                await storage.SaveSettings(settings);
                return true;
            }
            else
                return false;
        }
    }
}
