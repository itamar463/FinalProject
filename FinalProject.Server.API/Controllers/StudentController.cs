using Microsoft.AspNetCore.Mvc;
using FinalProject.Server.API.Repositories;
using FinalProject.Server.API.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FinalProject.Server.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private IPersonsReposetory repo;

        public StudentController() {
            repo = StudentsRepository.Instance;
            if (repo.Persons.Length == 0)
            {
                Student s1 = new Student("DolandDuck", 60, "Computer Science", false);
                Student s2 = new Student("MickyMouse", 30, "Computer Science", false);
                repo.AddPerson(s1);
                repo.AddPerson(s2);
            }
        }

            // GET: api/<StudentController>
        [HttpGet]
        public IEnumerable<Person> Get()
        {
            return this.repo.Persons;
        }

        // GET api/<StudentsController>/5
        [HttpGet("{id}")]
        public Person Get(string id)
        {
            Person? student = this.repo.Persons.Where(s => s.Id == id).SingleOrDefault();
            if (student != null)
                return student;
            else
                return new Student { Id = "-1" };
        }

        // POST api/<StudentsController>
        [HttpPost]
        public void Post(Person newStudent)
        {
            if (newStudent.Id == "")
            {
                newStudent.Id = Guid.NewGuid().ToString();
                repo.AddPerson(newStudent);
            }
        }

        // PUT api/<StudentsController>/5
        [HttpPut("{id}")]
        public void Put(string id, [FromBody] Person studentUpdate)
        {
            Person? student = this.repo.Persons.Where(s => s.Id == id).SingleOrDefault();
            if (student != null)
            {
                repo.UpdatePerson(studentUpdate);
            }




        }

        // PUT api/<StudentsController>/5
        [HttpPut]
        public void Put([FromBody] Person studentUpdate)
        {
            Person? student = this.repo.Persons.Where(s => s.Id == studentUpdate.Id).SingleOrDefault();
            if (student != null)
            {
                repo.UpdatePerson(studentUpdate);
            }




        }

        // DELETE api/<StudentsController>/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            Person? student = this.repo.Persons.Where(s => s.Id == id).SingleOrDefault();
            if (student != null)
            {
                repo.RemovePerson(id);
            }
        }
    }
}
