using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalProject.Server.API.Models;

namespace FinalProject.Server.API.Repositories
{
    internal interface IPersonsReposetory
    {
        //interface for funcationality, basic for student and teacher as well
        void AddPerson(Person person);
        void UpdatePerson(Person person);
        void RemovePerson(string id);
        //string LoadAllStudents();
        //void SaveAllStudents();
        Person[] Persons { get; }

        void SearchExamByName(string examName);
    }
}
