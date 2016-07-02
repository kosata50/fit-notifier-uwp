using FitNotifier.Data.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FitNotifier.Data.Storage
{
    public class SettingsStorage
    {
        private static readonly string AccountName = "KOS_account";
        private static readonly string FileName = "settings.xml";

        public async Task SaveSettings(Settings settings)
        {
            if (settings.User != null)
            {
                var vault = new Windows.Security.Credentials.PasswordVault();
                vault.Add(new Windows.Security.Credentials.PasswordCredential(AccountName, settings.User.Username, settings.User.Password));
            }

            var file = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFileAsync(FileName, Windows.Storage.CreationCollisionOption.OpenIfExists);
            using (Stream stream = await file.OpenStreamForWriteAsync())
            {
                XmlSerializer serializer = new XmlSerializer(settings.Entries.GetType());
                serializer.Serialize(stream, settings.Entries);
            }
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

            var folder = Windows.Storage.ApplicationData.Current.LocalFolder;
            if (await folder.TryGetItemAsync(FileName) != null)
            {
                var file = await folder.GetFileAsync(FileName);
                using (Stream stream = await file.OpenStreamForReadAsync())
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(SettingsEntries));
                    settings.Entries = (SettingsEntries)serializer.Deserialize(stream);
                }
            }
            else
            {
                settings.Entries = new SettingsEntries();
            }
            return settings;
        }

        public async Task DeleteSettings(Settings settings)
        {
            var vault = new Windows.Security.Credentials.PasswordVault();
            vault.Remove(new Windows.Security.Credentials.PasswordCredential(AccountName, settings.User.Username, settings.User.Password));

            var folder = Windows.Storage.ApplicationData.Current.LocalFolder;
            if (await folder.TryGetItemAsync(FileName) != null)
            {
                var file = await folder.GetFileAsync(FileName);
                await file.DeleteAsync();
            }
        }
    }
}
