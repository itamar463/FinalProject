using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FinalProject.Demos.Objects
{
    public class ExamData
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("studentId")]
        public string StudentId { get; set; }
        [JsonPropertyName("studentName")]
        public string StudentName { get; set; }
        [JsonPropertyName("examId")]
        public string ExamId { get; set; }
        [JsonPropertyName("grade")]
        public float Grade { get; set; }
        [JsonPropertyName("questionDetails")]
        public string QuestionDetails { get; set; }
        // will contain in one string the q name picked answer and correct answer
        // delimiter for questions : ***
        //delimiter for q name,picked answer,right answer : ^^^
        [JsonPropertyName("questionCount")]
        public int QuestionCount { get; set; }
        public ExamData() : this("", "", "", 0, "", -1)
        {

        }
        public ExamData(string studentId, string studentName, string examId, float grade, string questionDetails, int questionCount)
        {
            Id = Guid.NewGuid().ToString();
            StudentId = studentId;
            StudentName = studentName;
            ExamId = examId;
            Grade = grade;
            QuestionDetails = questionDetails;
            QuestionCount = questionCount;
        }
    }
}
