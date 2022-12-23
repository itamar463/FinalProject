using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Objects
{
    internal class Teacher : Person
    {
        //Teacher class harited from Person
        private List<Exam> exams; //hold his own exams as well need to see if to save here or on db

        public Teacher() : this("", -1, "")
        {

        }
        public Teacher(string name, int age, string fac) : base(name, age, fac)
        {
            exams = new List<Exam>();
        }

        
    }
}
