using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Demos.Objects
{
    public class Question
    {
        
        public string Id { get;}
        public bool IsImage { get; set; }
        public string QuestionContent { get; set; }
        public Dictionary<string, bool> Answers { get; set; }
        public float Weight { get; set; }


        public Question() : this(false, "", new Dictionary<string, bool>(), -1)
        {

        }
        public Question(bool isImage, string questionContent, Dictionary<string, bool> answers, float weight)
        {
            IsImage = isImage;
            QuestionContent = questionContent;
            Answers = answers;
            Weight = weight;
        }
    }
}
