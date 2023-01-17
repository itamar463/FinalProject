using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string Faculty { get; set; }
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
        public Person() : this("", -1, "", "",false)
        {

        }
        public Person(string name, int age, string fac, string pass,bool whoAmI)
        {
            this.Name = name;
            this.Age = age;
            //this.Id = Guid.NewGuid().ToString();
            this.Faculty = fac;
            this.Password = pass;
            this.IsTeacher = whoAmI;
        }

        public override string ToString()
        {
            return this.Name;
        }

    }
}
