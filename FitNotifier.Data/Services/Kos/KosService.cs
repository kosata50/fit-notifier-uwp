using FitNotifier.Data.Services.Kos.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FitNotifier.Data.Services.Kos
{
    public class KosService
    {
        public KosCredencials Credencials { get; private set; }

        private HttpClient client;
        private XNamespace nsAtom;
        private XNamespace nsKosSchema;
        private XNamespace nsLink;

        private static string ApiBase = "https://kosapi.fit.cvut.cz/api/3/";

        public KosService(KosCredencials credencials)
        {
            Credencials = credencials;
            client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Credencials.TokenType, Credencials.AccessToken);
            nsAtom = XNamespace.Get("http://www.w3.org/2005/Atom");
            nsKosSchema = XNamespace.Get("http://kosapi.feld.cvut.cz/schema/3");
            nsLink = XNamespace.Get("http://www.w3.org/1999/xlink");
        }

        public async Task<People> GetPeople(string username)
        {
            string url = GetResourceUrl("/people/{0}", username);

            string data = await client.GetStringAsync(url);
            XDocument document = XDocument.Parse(data);

            XElement content = document.Root.Element(nsAtom + "content");
            var t  = content.Elements().ToList();
            return new People()
            {
                Title = document.Root.Element(nsAtom + "title").Value,
                FirstName = content.Element(nsKosSchema + "firstName").Value,
                LastName = content.Element(nsKosSchema + "lastName").Value,
                PersonalNumber = int.Parse(content.Element(nsKosSchema + "personalNumber").Value),
                Username = content.Element(nsKosSchema + "username").Value
            };
        }

        public async Task<List<Course>> GetCourses(string username)
        {
            string url = GetResourceUrl("/students/{0}/enrolledCourses", username);

            List<Course> courses = new List<Course>();
            foreach (XElement entry in await GetAllEntries(url))
            {
                XElement content = entry.Element(nsAtom + "content");
                courses.Add(new Course()
                {
                    Name = content.Element(nsKosSchema + "course").Value,
                    Code = GetValueFromHref(content.Element(nsKosSchema + "course")),
                    Semester = content.Element(nsKosSchema + "semester").Value,
                    SemesterCode = GetValueFromHref(content.Element(nsKosSchema + "semester")),
                    Assessment = bool.Parse(content.Element(nsKosSchema + "assessment").Value),
                    Completed = bool.Parse(content.Element(nsKosSchema + "completed").Value)
                });
            }
            courses.Sort(Comparer<Course>.Create((c1, c2) => c1.Code.CompareTo(c2.Code)));
            return courses;
        }

        public async Task<List<Exam>> GetExams(string courseCode)
        {
            string url = GetResourceUrl("courses/{0}/exams", courseCode);

            List <Exam> exams = new List<Exam>();
            foreach (XElement entry in await GetAllEntries(url))
            {
                XElement content = entry.Element(nsAtom + "content");
                XElement endDate = content.Element(nsKosSchema + "endDate");
                exams.Add(new Exam()
                {
                    ID = entry.Element(nsAtom + "id").Value.Split(':')[4],
                    CourseCode = GetValueFromHref(content.Element(nsKosSchema + "course")),
                    Capacity = int.Parse(content.Element(nsKosSchema + "capacity").Value),
                    Occupied = int.Parse(content.Element(nsKosSchema + "occupied").Value),
                    Room = content.Element(nsKosSchema + "room")?.Value,
                    Note = content.Element(nsKosSchema + "note")?.Value,
                    Start = DateTime.Parse(content.Element(nsKosSchema + "startDate").Value),
                    End = (endDate != null) ? DateTime.Parse(endDate.Value) : (DateTime?)null,
                    Type = (ExamType)Enum.Parse(typeof(ExamType), content.Element(nsKosSchema + "termType").Value.Replace("_", ""), true)
                });
            }
            return exams;
        }

        public async Task<List<ExamRegistration>> GetExamRegistrations(string username)
        {
            string url = GetResourceUrl("students/{0}/registeredExams", username);

            List<ExamRegistration> registations = new List<ExamRegistration>();
            foreach (XElement entry in await GetAllEntries(url))
            {
                XElement content = entry.Element(nsAtom + "content");
                registations.Add(new ExamRegistration()
                {
                    ID = GetValueFromHref(content.Element(nsKosSchema + "exam"))
                });
            }
            return registations;
        }


        private string GetValueFromHref(XElement element)
        {
            return element.Attribute(nsLink + "href").Value.Split('/')[1];
        }

        private async Task<List<XElement>> GetAllEntries(string url)
        {
            List<XElement> entries = new List<XElement>();
            XDocument document;
            do
            {
                document = XDocument.Parse(await client.GetStringAsync(url));
                foreach (XElement entry in document.Root.Descendants(nsAtom + "entry"))
                    entries.Add(entry);

                url = document.Root.Descendants(nsAtom + "link").SingleOrDefault(l => l.Attribute("rel").Value == "next")?.Attribute("href").Value;
                if (url != null)
                    url = GetResourceUrl(url);
            } while (url != null);
            return entries;
        }

        private string GetResourceUrl(string url, params object[] param)
        {
            return ApiBase + string.Format(url, param);
        }
    }
}
