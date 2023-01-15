﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Demos.Objects
{
    public class Student : Person
    {
        //student class heritad from Person
        public Student() : this("", -1, "", "", false)
        {

        }
        public Student(string name, int age, string fac, string pass, bool whoAmI) : base(name, age, fac, pass, whoAmI)
        {

        }



    }
}
