using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FinalProject.Demos.Objects
{
    // Base class for one question
    public class Question
    {
        [JsonPropertyName ("id")]
        public string Id { get; set; }
        [JsonPropertyName("isImage")]
        public bool IsImage { get; set; }
        [JsonPropertyName("questionContent")]
        public string QuestionContent { get; set; }
       
        [JsonPropertyName("weight")]
        public float Weight { get; set; }
        [JsonPropertyName("examId")]
        public string ExamId { get; set; }
        [JsonPropertyName("answer1")]
        public string Answer1 { get; set; }
        [JsonPropertyName("answer2")]
        public string Answer2 { get; set; }
        [JsonPropertyName("answer3")]
        public string Answer3 { get; set; }
        [JsonPropertyName("answer4")]
        public string Answer4 { get; set; }
        [JsonPropertyName("correctAnswer")]
        public int CorrectAnswer { get; set; }
        [JsonPropertyName("questionNumber")]
        public int QuestionNumber { get; set; }
        [JsonPropertyName("imageData")]
        public byte[] ImageData { get; set; }

        public Question() : this(false, "", -1, -1, "")
        {

        }
        public Question(bool isImage, string questionContent, float weight, int questionNumber, string exmaid)
        {
            IsImage = isImage;
            QuestionContent = questionContent;
            // Answers = answers;
            Weight = weight;
            QuestionNumber = questionNumber;
            ExamId = exmaid;
        }
        public override string ToString()
        {
            return "Question " + QuestionNumber.ToString();
        }

    }
}
