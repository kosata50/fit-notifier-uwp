using FitNotifier.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitNotifier.Data.Model
{
    public class Settings
    {
        public UserCredencials User { get; set; }
        public SettingsEntries Entries { get; set; }

        public bool LoggedIn => User != null;
    }
}
