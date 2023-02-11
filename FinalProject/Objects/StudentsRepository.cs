using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FinalProject.Demos.Objects
{
    //This class handles the list of students, and it's structure is mainly inharited from IPersonRepository Interface
    public class StudentsRepository : IPersonsReposetory
    {
        //data handling for students need to see how it works with db and so on...
        private List<Student> _students;
        static private StudentsRepository _instance = null;
        HttpClient clientApi;

        private StudentsRepository()
        {
            _students = new List<Student>();
            clientApi = new HttpClient();
            clientApi.BaseAddress = new Uri("https://localhost:7277");
        }

        //Get Factory Of StudentsRepository  as singelton

        public static StudentsRepository Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new StudentsRepository();

                }
                return _instance;
            }
        }
        public List<Student> GetStudents()
        {
            var response = clientApi?.GetAsync("api/users").Result;
            string? dataString = response?.Content.ReadAsStringAsync().Result;
            List<Person>? persons = JsonSerializer.Deserialize<List<Person>>(dataString);
            List<Student> load_students = new List<Student>();
            
            foreach(var p in persons)
            {
                if (!p.IsTeacher)
                {
                    Student student = new Student();
                    student.Age = p.Age;
                    student.IsTeacher = p.IsTeacher;
                    student.Id = p.Id;
                    student.Password = p.Password;
                    student.Name = p.Name;
                    load_students.Add(student);
                }
            }
            return load_students;
        }

        public Student[] Students
        {
            get
            {
                var st = GetStudents();


                return st.ToArray();
            }

        }
       
        public void AddPerson(Person person)
        {
            if (person is Student)
            {
                Student student = (Student)person;
                this._students.Add(student);
            }
        }
        
        public void RemovePerson(string id)
        {
            int indexFound = this._students.FindIndex(s => s.Id == id);
            if (indexFound >= 0)
            {
                this._students.RemoveAt(indexFound);
            }
        }

        public void SearchExamByName(string examName)
        {
            throw new NotImplementedException();
        }

        public void UpdatePerson(Person person)
        {
            Student st = (Student)person;
            int indexFound = this._students.FindIndex(s => s.Id == st.Id);
            if (indexFound >= 0)
            {
                this._students[indexFound] = st;

            }
        }
    }
}
