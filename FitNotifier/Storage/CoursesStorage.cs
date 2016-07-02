using FitNotifier.Data.Services.Kos.Entities;
using FitNotifier.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FitNotifier.Storage
{
    public class CoursesStorage
    {
        private static string FileName = "courses.xml";

        public async Task SaveCourses(List<CourseItem> courses)
        {
            var folder = Windows.Storage.ApplicationData.Current.LocalFolder;
            var file = await folder.CreateFileAsync(FileName, Windows.Storage.CreationCollisionOption.ReplaceExisting);
            using (Stream stream = await file.OpenStreamForWriteAsync())
            {
                XmlSerializer serializer = new XmlSerializer(courses.GetType());
                serializer.Serialize(stream, courses);
            }
        }

        public async Task<List<CourseItem>> LoadCourses()
        {
            List<CourseItem> courses = new List<CourseItem>();
            var folder = Windows.Storage.ApplicationData.Current.LocalFolder;
            if (await folder.TryGetItemAsync(FileName) != null)
            {
                var file = await folder.GetFileAsync(FileName);
                using (Stream stream = await file.OpenStreamForReadAsync())
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<CourseItem>));
                    courses = (List<CourseItem>)serializer.Deserialize(stream);
                }
            }
            return courses;
            
        }

        public async Task DeleteCourses()
        {
            var folder = Windows.Storage.ApplicationData.Current.LocalFolder;
            if (await folder.TryGetItemAsync(FileName) != null)
            {
                var file = await folder.GetFileAsync(FileName);
                await file.DeleteAsync();
            }
        }
    }
}
