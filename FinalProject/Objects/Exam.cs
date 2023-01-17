using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Demos.Objects
{
    internal class Exam
    {
        
        public string Name { get; set; }
        public string Id { get; set; }
        public DateTime StratDate { get; set; }
        public DateTime EndDate { get; set; }
        public Teacher Teacher { get; set; }
        public float Totaltime { get; set; }
        public bool IsRandomize { get; set; } //maybe we don't need it?
        public List<Question> Questions { get; set; }
        public float Grade { get; set; }


        public Exam() : this("", "", DateTime.Now, DateTime.Now, new Teacher(), -1, false, new List<Question>(), -1)
        {

        }
        public Exam(string name, string id, DateTime stratDate, DateTime endDate, Teacher teacher, float totaltime, bool isRandomize, List<Question> questions, float grade)
        {
            Name = name;
            Id = Guid.NewGuid().ToString(); ;
            StratDate = stratDate;
            EndDate = endDate;
            Teacher = teacher;
            Totaltime = totaltime;
            IsRandomize = isRandomize;
            Questions = questions;
            Grade = grade;
        }
        public void mixQuestions()
        {
            if (IsRandomize)
            {
                Random rand = new Random();
                var shuffled = Questions.OrderBy(_ => rand.Next()).ToList();
                Questions = shuffled;
            }
        }
    }
}
