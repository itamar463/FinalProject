using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Server.API.Models
{
    [Table("Exams")]
    public class Exam
    {
        public string Name { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public DateTime StratDate { get; set; }
        public DateTime EndDate { get; set; }
        public string TeacherId { get; set; }
        public float Totaltime { get; set; }
        public bool IsRandomize { get; set; }
        public float Grade { get; set; }


        public Exam() : this("","", DateTime.Now,DateTime.Now,new Teacher(),-1,false,-1)
        {

        }
        public Exam(string name, string id, DateTime stratDate, DateTime endDate, Teacher teacher, float totaltime, bool isRandomize, float grade)
        {
            Name = name;
            StratDate = stratDate;
            EndDate = endDate;
            TeacherId = teacher.Id;
            Totaltime = totaltime;
            IsRandomize = isRandomize;
            Grade = grade;
        }
        public void mixQuestions()
        {
            if (IsRandomize)
            {
                Random rand = new Random();
            }
        }
    }
}
