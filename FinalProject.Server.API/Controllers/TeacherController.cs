using Microsoft.AspNetCore.Mvc;
using FinalProject.Server.API.Repositories;
using FinalProject.Server.API.Models;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FinalProject.Server.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private IPersonsReposetory repo;

        public TeacherController()
        {
            repo = TeachersRepository.Instance;
            if (repo.Persons.Length == 0)
            {
                Teacher s1 = new Teacher("Donald", 60, "Computer Science", "1234",true);
                Teacher s2 = new Teacher("Micky", 30, "Computer Science", "1234",true);
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
            Person? teacher = this.repo.Persons.Where(t => t.Id == id).SingleOrDefault();
            if (teacher != null)
                return teacher;
            else
                return new Teacher { Id = "-1" };
        }

        // POST api/<StudentsController>
        [HttpPost]
        public void Post(Teacher newTeacher)
        {
            if (newTeacher.Id == "string")
            {
                newTeacher.Id = Guid.NewGuid().ToString();
                repo.AddPerson(newTeacher);
            }
        }

        // PUT api/<StudentsController>/5
        [HttpPut("{id}")]
        public void Put(string id, [FromBody] Teacher teacherUpdate)
        {
            //maybe we dont need it or the other put
            Person? teacher = this.repo.Persons.Where(t => t.Id == id).SingleOrDefault();
            if (teacher != null)
            {
                repo.UpdatePerson(teacherUpdate);
            }
        }

        // PUT api/<StudentsController>/5
        [HttpPut]
        public void Put([FromBody] Teacher teacherUpdate)
        {
            Person? teacher = this.repo.Persons.Where(s => s.Id == teacherUpdate.Id).SingleOrDefault();
            if (teacher != null)
            {
                repo.UpdatePerson(teacherUpdate);
            }




        }

        // DELETE api/<StudentsController>/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            Person? teacher = this.repo.Persons.Where(t => t.Id == id).SingleOrDefault();
            if (teacher != null)
            {
                repo.RemovePerson(id);
            }
        }
    }
}
