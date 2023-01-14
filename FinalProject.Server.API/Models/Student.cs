using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Server.API.Models
{
    internal class Student : Person
    {
        //student class heritad from Person
        public Student() : this("", -1, "",false)
        {

        }
        public Student(string name, int age, string fac,bool whoAmI) : base(name, age, fac, whoAmI)
        {
            
        }

        
    }
}
