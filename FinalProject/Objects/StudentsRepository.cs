using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Objects
{
    internal class StudentsRepository : IPersonsReposetory
    {
        //data handling for students need to see how it works with db and so on...
        private List<Student> students;

        public StudentsRepository()
        {
            students = new List<Student>();
        }
        public void AddPerson(Person person)
        {
           //maybe to do with sql and db ?
        }

        public Person[] Persons() 
        {
             return students.ToArray(); 
        }

        public void RemovePerson(string id)
        {
            //maybe to do with sql and db ?
        }

        public void SearchExamByName(string examName)
        {
            throw new NotImplementedException();
        }

        public void UpdatePerson(Person person)
        {
            //maybe to do with sql and db ?
        }
    }
}
