using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Server.API.Models
{
    public class ExamData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string StudentId { get; set; }
        public string StudentName { get; set; }

        public string ExamId { get; set; }

        public float Grade { get; set; }

        public string QuestionDetails { get; set; }
        // will contain in one string the q name picked answer and correct answer
        // delimiter for questions : ***
        //delimiter for q name,picked answer,right answer : ^^^
        public int QuestionCount { get; set; }
        public ExamData() : this("", "", "", 0, "", -1)
        {

        }
        public ExamData(string studentId, string studentName, string examId, float grade, string questionDetails, int questionCount)
        {
            
            StudentId = studentId;
            StudentName = studentName;
            ExamId = examId;
            Grade = grade;
            QuestionDetails = questionDetails;
            QuestionCount = questionCount;
        }
    }
}
