using FinalProject.Server.API.Models;

namespace FinalProject.Server.API.Repositories
{
    internal class TeachersRepository : IPersonsReposetory
    {
        
        List<Teacher> teachers;
        static private TeachersRepository _instance = null;

        private TeachersRepository()
        {
            teachers = new List<Teacher>();
        }

        //Get Factory Of TeachersRepository  as singelton

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

        Person[] IPersonsReposetory.Persons => teachers.ToArray();


        public void AddPerson(Person person)
        {
            if (person is Teacher)
            {
                teachers.Add((Teacher)person);
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
