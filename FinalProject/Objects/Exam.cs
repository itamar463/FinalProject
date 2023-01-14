using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Demos.Objects
{
    internal class Exam
    {
        private string Name { get; set; }
        private string Id { get; set; }
        private DateTime StratDate { get; set; }
        private DateTime EndDate { get; set; }
        private Teacher Teacher { get; set; }
        private float Totaltime { get; set; }
        private bool IsRandomize { get; set; } //maybe we don't need it?
        private List<Question> Questions { get; set; }
        private float Grade { get; set; }


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
