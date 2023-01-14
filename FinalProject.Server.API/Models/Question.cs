using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Server.API.Models
{
    internal class Question
    {
        private bool IsImage { get; set; }
        private string QuestionContent { get; set; }
        private Dictionary<string,bool> Answers { get; set; }
        private float Weight { get; set; }


        public Question() : this(false, "", new Dictionary<string, bool>(),-1)
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
