using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Demos.Objects
{
    public class TeachersRepository : IPersonsReposetory
    {
        //data handling for students need to see how it works with db and so on...
        List<Teacher> teachers;

        public TeachersRepository()
        {
            teachers = new List<Teacher>();
        }

        //Person[] IPersonsReposetory.Persons => throw new NotImplementedException(); // need to change

        public void AddPerson(Person person)
        {
            //maybe to do with sql and db ?
        }

        public Person[] Persons()
        {
            return teachers.ToArray();
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
