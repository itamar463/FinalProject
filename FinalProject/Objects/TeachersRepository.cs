using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FinalProject.Demos.Objects
{
    public class TeachersRepository : IPersonsReposetory
    {
        //data handling for students need to see how it works with db and so on...
        List<Teacher> teachers;
        static private TeachersRepository _instance = null;
        HttpClient clientApi;
        public TeachersRepository()
        {
            teachers = new List<Teacher>();
            clientApi = new HttpClient();
            clientApi.BaseAddress = new Uri("https://localhost:7277");
        }
        //03 Get Factory Of TeachersRepository  as singelton

        public static TeachersRepository Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TeachersRepository();

                }
                return _instance;
            }
        }


        public void AddPerson(Person person)
        {
         if(person is Teacher)
            {

                teachers.Add((Teacher)person);
            }   
        }

        public List<Teacher> GetTeachers()
        {
            var response = clientApi?.GetAsync("api/users").Result;
            //  response.EnsureSuccessStatusCode();
            string? dataString = response?.Content.ReadAsStringAsync().Result;
            var load_teachers = JsonSerializer.Deserialize<List<Teacher>>(dataString);
            return load_teachers;
        }
        public Teacher[] Teachers
        {
            get
            {
                var teach = GetTeachers();


                return teach.ToArray();
            }

        }

        public void RemovePerson(string id)
        {
            int indexFound = this.teachers.FindIndex(t => t.Id == id);
            if (indexFound >= 0)
            {
                this.teachers.RemoveAt(indexFound);
            }
        }

        public void SearchExamByName(string examName)
        {
            throw new NotImplementedException();
        }

        public void UpdatePerson(Person person)
        {
            Teacher t = (Teacher)person;
            int indexFound = this.teachers.FindIndex(s => s.Id == t.Id);
            if (indexFound >= 0)
            {
                this.teachers[indexFound] = t;

            }
        }
    }
}
