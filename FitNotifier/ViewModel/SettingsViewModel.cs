using FitNotifier.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitNotifier.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        public Settings Settings { get; private set; }

        public string Username { get; set; }
        public string Password { get; set; }

        public bool LoggedIn => Settings.LoggedIn;
        public bool LoginProgress { get; private set; }

        public SettingsViewModel(Settings settings)
        {
            Settings = settings;
            Username = Settings.User?.Username;
            Password = Settings.User?.Password;
        }

        public void SetLoginProgress(bool progress)
        {
            LoginProgress = progress;
            OnPropertyChanged(nameof(LoginProgress));
        }

        public void Logout()
        {
            Settings.User = null;
            OnPropertyChanged(nameof(LoggedIn));
        }
    }
}
