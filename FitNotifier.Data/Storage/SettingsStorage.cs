using FitNotifier.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitNotifier.Data.Storage
{
    public class SettingsStorage
    {
        private static string AccountName = "KOS_account";

        public async Task SaveSettings(Settings settings)
        {
            if (settings.User != null)
            {
                var vault = new Windows.Security.Credentials.PasswordVault();
                vault.Add(new Windows.Security.Credentials.PasswordCredential(AccountName, settings.User.Username, settings.User.Password));
            }
            await Task.Delay(0);//Varning suppress
        }

        public async Task<Settings> LoadSettings()
        {
            Settings settings = new Settings();
            var vault = new Windows.Security.Credentials.PasswordVault();
            var credencials = vault.RetrieveAll().SingleOrDefault(c => c.Resource == AccountName);
            if (credencials != null)
            {
                credencials.RetrievePassword();
                settings.User = new Services.UserCredencials(credencials.UserName, credencials.Password);
            }
            await Task.Delay(0);//Varning suppress
            return settings;
        }

        public async Task DeleteSettings(Settings settings)
        {
            var vault = new Windows.Security.Credentials.PasswordVault();
            vault.Remove(new Windows.Security.Credentials.PasswordCredential(AccountName, settings.User.Username, settings.User.Password));
            await Task.Delay(0);//Varning suppress
        }
    }
}
