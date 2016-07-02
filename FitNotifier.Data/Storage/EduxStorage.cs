using FitNotifier.Data.Services.Edux.Entities;
using FitNotifier.Data.Services.Kos.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace FitNotifier.Data.Storage
{
    public class EduxStorage
    {
        private static string FolderName = "Edux";

        private async Task<StorageFolder> GetFolder()
        {
            var local = ApplicationData.Current.LocalFolder;
            return await local.CreateFolderAsync(FolderName, CreationCollisionOption.OpenIfExists);
        }

        public async Task SaveClassification(EduxClassification classifiction)
        {
            var folder = await GetFolder();
            var file = await folder.CreateFileAsync(classifiction.CourseCode, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, classifiction.HTML ?? string.Empty);
        }

        public async Task<EduxClassification> LoadClassification(Course course)
        {
            var folder = await GetFolder();
            if (await folder.TryGetItemAsync(course.Code) != null)
            {
                var file = await folder.GetFileAsync(course.Code);
                return new EduxClassification()
                {
                    CourseCode = course.Code,
                    HTML = await FileIO.ReadTextAsync(file)
                };
            }
            return new EduxClassification() { CourseCode = course.Code };
        }

        public async Task DeleteClassification(Course course)
        {
            var folder = await GetFolder();
            if (await folder.TryGetItemAsync(course.Code) != null)
            {
                var file = await folder.GetFileAsync(course.Code);
                await file.DeleteAsync();
            }
        }

        public async Task DeleteAllClassifications()
        {
            var folder = await GetFolder();
            foreach (var file in await folder.GetFilesAsync())
                await file.DeleteAsync();
        }
    }
}
