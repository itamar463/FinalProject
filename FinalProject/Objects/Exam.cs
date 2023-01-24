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
        [JsonPropertyName("teacherId")]
        public string TeacherId { get; set; }
        [JsonPropertyName("totaltime")]
        public float Totaltime { get; set; }
        [JsonPropertyName("isRandomize")]
        public bool IsRandomize { get; set; } //maybe we don't need it?
        
       
        [JsonPropertyName("grade")]
        public float Grade { get; set; }


        public Exam() : this("", "", DateTime.Now, DateTime.Now, new Teacher(), -1, false, new List<Question>(), -1)
        {

        }
        public Exam(string name, string id, DateTime stratDate, DateTime endDate, Teacher teacher, float totaltime, bool isRandomize, List<Question> questions, float grade)
        {
            Name = name;
            Id = Guid.NewGuid().ToString(); 
            StratDate = stratDate;
            EndDate = endDate;
            TeacherId = teacher.Id;
            Totaltime = totaltime;
            IsRandomize = isRandomize;
            Grade = grade;
        }
        public void mixQuestions()
        {
            
        }

        public override string ToString()
        {
            return Name.ToString();
        }
    }
}
