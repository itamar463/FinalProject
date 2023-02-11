using FinalProject.Server.API.Models;

namespace FinalProject.Server.API.Repositories
{
    public interface IPersonsReposetory
    {
        //interface for funcationality, basic for student and teacher as well
        void AddPerson(Person person);
        void UpdatePerson(Person person);
        void RemovePerson(string id);

        Person[] Persons { get; }

        void SearchExamByName(string examName);
    }
}
