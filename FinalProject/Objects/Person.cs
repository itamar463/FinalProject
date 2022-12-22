﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Objects
{
    internal class Person 
    {
        //base class for teacher and student
        private string Name { get; set; }
        private string Id { get; set; }

        private int age = -1;
        private string Faculty { get; set; }
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
        
        public Person() : this("", -1, "")
        {

        }
        public Person(string name, int age, string fac)
        {
            this.Name = name;
            this.Age = age;
            this.Id = Guid.NewGuid().ToString();
            this.Faculty = fac;
        }

        public override string ToString()
        {
            return this.Name;
        }

    }
}
