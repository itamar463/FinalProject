using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FinalProject.Demos.Objects
{
    public class Exam
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("startDate")]
        public DateTime StratDate { get; set; }
        [JsonPropertyName("endDate")]
        public DateTime EndDate { get; set; }
        [JsonPropertyName("teacher")]
        public Teacher Teacher { get; set; }
        [JsonPropertyName("totalTime")]
        public float Totaltime { get; set; }
        [JsonPropertyName("isRandomize")]
        public bool IsRandomize { get; set; } //maybe we don't need it?
        public List<Question> Questions { get; set; }
       
        [JsonPropertyName("grade")]
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
