

namespace FinalProject.Server.API.Models
{
    public class Teacher : Person
    {
        //Teacher class harited from Person
        private List<Exam> Exams { get; set; } //hold his own exams as well need to see if to save here or on db

        public Teacher() : this("", -1, "", true)
        {

        }
        public Teacher(string name, int age, string pass, bool whoAmI) : base(name, age, pass, whoAmI)
        {
            Exams = new List<Exam>();
        }


    }
}
