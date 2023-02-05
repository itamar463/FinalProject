using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FinalProject.Demos.Objects
{
    public class Person
    {
        //base class for teacher and student
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("id")]
        public string Id { get; set; }

        public int age = -1;
        
        [JsonPropertyName("password")]
        public string Password { get; set; } 
        [JsonPropertyName("age")]
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
        [JsonPropertyName("isTeacher")]
        public bool IsTeacher { get; set; }
        public Person() : this("", -1, "", false)
        {

        }
        public Person(string name, int age, string pass, bool whoAmI)
        {
            Name = name;
            Age = age;
            Id = Guid.NewGuid().ToString();
            Password = pass; 
            IsTeacher = whoAmI;
        }

        public override string ToString()
        {
            return Name;
        }

    }
}
