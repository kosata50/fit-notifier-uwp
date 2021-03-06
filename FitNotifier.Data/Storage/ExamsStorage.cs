﻿using FitNotifier.Data.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Storage;

namespace FitNotifier.Data.Storage
{
    public class ExamsStorage
    {
        private static string FolderName = "Exams";

        public async Task SaveExams(List<ExamInfo> exams, string courseCode)
        {
            var folder = await GetFolder();
            var file = await folder.CreateFileAsync(courseCode, CreationCollisionOption.ReplaceExisting);
            using (Stream stream = await file.OpenStreamForWriteAsync())
            {
                XmlSerializer serializer = new XmlSerializer(exams.GetType());
                serializer.Serialize(stream, exams);
            }
        }

        public async Task<List<ExamInfo>> LoadExams(string courseCode)
        {
            var folder = await GetFolder();
            if (await folder.TryGetItemAsync(courseCode) != null)
            {
                var file = await folder.GetFileAsync(courseCode);
                using (Stream stream = await file.OpenStreamForReadAsync())
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<ExamInfo>));
                    return (List<ExamInfo>)serializer.Deserialize(stream);
                }
            }
            else
                return new List<ExamInfo>();
        }

        public async Task DeleteExams(string courseCode)
        {
            var folder = await GetFolder();
            if (await folder.TryGetItemAsync(courseCode) != null)
            {
                var file = await folder.GetFileAsync(courseCode);
                await file.DeleteAsync();
            }
        }

        public async Task DeleteAllExams()
        {
            var folder = await GetFolder();
            foreach (var file in await folder.GetFilesAsync())
                await file.DeleteAsync();
        }

        private async Task<StorageFolder> GetFolder()
        {
            var local = ApplicationData.Current.LocalFolder;
            return await local.CreateFolderAsync(FolderName, CreationCollisionOption.OpenIfExists);
        }
    }
}
