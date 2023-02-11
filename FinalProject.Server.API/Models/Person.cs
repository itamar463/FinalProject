using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Server.API.Models
{
    [Table("Users")]
    public class Person 
    {
        //base class for teacher and student
        public string Name { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public int age = -1;
       
        public int Age
        {
            get { return age; }
            set
            {
                if (value > 18)
                {
                    age = value;
                }

            }
        }
        public string Password { get; set; }
        public bool IsTeacher { get; set; }
        public Person() : this("", -1, "",false)
        {

        }
        public Person(string name, int age, string pass,bool whoAmI)
        {
            this.Name = name;
            this.Age = age;
            //this.Id = Guid.NewGuid().ToString();
            this.Password = pass;
            this.IsTeacher = whoAmI;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
