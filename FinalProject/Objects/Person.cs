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
        [JsonPropertyName("faculty")]
        public string Faculty { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; } //added password
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
        public bool IsTeacher { get; set; }
        public Person() : this("", -1, "", "", false)
        {

        }
        public Person(string name, int age, string fac,string pass, bool whoAmI)
        {
            Name = name;
            Age = age;
            Id = Guid.NewGuid().ToString();
            Faculty = fac;
            Password = pass; // added password
            IsTeacher = whoAmI;
        }

        public override string ToString()
        {
            return Name;
        }

    }
}
