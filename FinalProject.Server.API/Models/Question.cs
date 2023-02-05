using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Server.API.Models
{
    
    public class Question
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; } 
        public bool IsImage { get; set; }
        public string QuestionContent { get; set; }
        public string Answer1 { get; set; }
        public string Answer2 { get; set; }
        public string Answer3 { get; set; }
        public string Answer4 { get; set; }
        public int CorrectAnswer { get; set; }
        public float Weight { get; set; }
        public string ExamId { get; set; }
        public int QuestionNumber { get; set; }
        public byte[] ImageData { get; set; }

        public Question() : this(false, "",-1,-1,"")
        {

        }
        public Question(bool isImage, string questionContent, float weight, int questionNumber,string exmaid)
        {
            IsImage = isImage;
            QuestionContent = questionContent;
           // Answers = answers;
            Weight = weight;
            QuestionNumber = questionNumber;
            ExamId = exmaid;
        }
    }
}
