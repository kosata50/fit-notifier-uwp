using FitNotifier.Data.Services.Edux.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FitNotifier.Data.Services.Edux
{
    public class EduxService
    {
        private UserCredencials user;
        private HttpClient client;

        private static readonly string LoginUrl = "https://edux.fit.cvut.cz/start?do=login";
        private static readonly string ClassificationTemplate = "https://edux.fit.cvut.cz/courses/{0}/_export/xhtml/classification/student/{1}/start";
        private static readonly string StartTag = "<table";
        private static readonly string EndTag = "</table>";

        public EduxService(UserCredencials user)
        {
            this.user = user;
            client = new HttpClient();
        }

        public async Task Login()
        {
            HttpContent content = new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                { "id", "start" },
                { "do", "login" },
                { "authnProvider", "1" },
                { "u", user.Username },
                { "p", user.Password }
            });
            HttpResponseMessage response = await client.PostAsync(LoginUrl, content);
            response.EnsureSuccessStatusCode();
        }

        public async Task<EduxClassification> GetClassification(string courseCode)
        {
            string url = string.Format(ClassificationTemplate, courseCode, user.Username);
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string html = await response.Content.ReadAsStringAsync();
                int start = html.IndexOf(StartTag);
                int length = html.LastIndexOf(EndTag) - start + EndTag.Length;
                if (start > 0 && length > 0)
                {
                    html = html.Substring(start, length);

                    return new EduxClassification()
                    {
                        CourseCode = courseCode,
                        HTML = html
                    };
                }
            }
            return new EduxClassification();
        }
    }
}
