

namespace FinalProject.Server.API.Models
{
    public class Student : Person
    {
        //student class heritad from Person
        public Student() : this("", -1, "",false)
        {

        }
        public Student(string name, int age, string pass,bool whoAmI) : base(name, age, pass,whoAmI)
        {
            
        }

        
    }
}
