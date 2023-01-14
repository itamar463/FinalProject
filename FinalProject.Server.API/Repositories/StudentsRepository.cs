using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalProject.Server.API.Models;
using FinalProject.Server.API.Controllers;

namespace FinalProject.Server.API.Repositories
{
    internal class StudentsRepository : IPersonsReposetory
    {
        //data handling for students need to see how it works with db and so on...
        private List<Student> _students;
        static private StudentsRepository _instance = null;

        //01 change to private
        private StudentsRepository()
        {
            _students = new List<Student>();
        }

        //03 Get Factory Of StudentsRepository  as singelton

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

        Person[] IPersonsReposetory.Persons => _students.ToArray();

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
