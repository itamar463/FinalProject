using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;
using FinalProject.Demos.Objects;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text.Json;
using Newtonsoft.Json;
using System.Windows.Markup;
using System.Threading;
using Azure;
using System.Collections;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Microsoft.Win32;
using System.IO;

namespace FinalProject.Demos
{
    
    public partial class AddExamWindow : Window
    {
        private float pointToGrade = 0; // check if enough points to set the full grade for the exam
        private int qNumber = 1; // the questions number by order
        private List<Question> _questions;
        private Exam exam;
        private HttpClient client;
        private string url = "https://localhost:7277/api/Questions";
        private string filePath = ""; //for image question new!!!
        private byte[]? imgData = null; //added
       // private BitmapImage bitmapImage; //added
        private Image imageShow;
        private bool isAnswerRand;
        
        public AddExamWindow(Exam exam, bool isArand)
        {
            InitializeComponent();
            _questions = new List<Question>();
            this.exam = exam;
            helloLbl.Content += exam.Name;
            client = new HttpClient();
            //bitmapImage = new BitmapImage();
            imageShow = new Image();
            isAnswerRand = isArand;
            GetQuestions();
        }

        //GetQuestions: if the exam is need update, takes the relative questions from data base
        private void GetQuestions()
        {
            
            var response = client?.GetAsync(url).Result;
            string? dataString = response?.Content.ReadAsStringAsync().Result;
            List<Question>? allQuestions = JsonSerializer.Deserialize<List<Question>>(dataString);
            if(allQuestions != null)
            {
                foreach (var item in allQuestions)
                {
                    if (item.ExamId == exam.Id)
                    {
                        _questions.Add(item);
                        pointToGrade += item.Weight;
                    }
                }
                _questions = _questions.OrderBy(x => x.QuestionNumber).ToList();

                qNumber = _questions.Count + 1; //the question number to continue with
                QuestionsLST.ItemsSource = _questions;
                QuestionsLST.Items.Refresh();
            }
        }
        //RemoveBtn_Click: remove selected question from exam(selected from listbox)
        private void RemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            
            if (this.QuestionsLST.SelectedItem is Question q)
            {   
                int curr_num = q.QuestionNumber;
                if(curr_num < qNumber)
                {
                    qNumber--;
                    while (curr_num < qNumber)
                    {
                        _questions[curr_num].QuestionNumber--;
                        curr_num++;
                    }
                    
                }
                _questions.Remove(q);
                QuestionsLST.ItemsSource = _questions;
                QuestionsLST.Items.Refresh();
                QuestionsLST.SelectedIndex = 0;
            }
        }
        //AddBtn_Click: add new question to the exam
        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
        
            //create new question
            var question = new Question();
            //to check that there is correct answer
            bool is_pressed = false;
            //check question content is not empty
            question.QuestionContent = QuestionTXT.Text;
            if (question.QuestionContent != "" || imgData != null)
            {
                //new!!!
                if (imgData != null)
                {
                    question.ImageData = imgData;
                    question.QuestionContent = filePath;
                    question.IsImage = true;
                }
                else
                {
                    question.IsImage = false;
                    question.ImageData = new byte[] {0x00};
                }
            }
            else
            {
                MessageBox.Show("Fill question content.");
                return;
            }
            try
            {
                //check question weight is valid and not empty
                if (QuestionWeight.Text != "")
                {
                    question.Weight = float.Parse(QuestionWeight.Text);
                    if (question.Weight <= 0)
                    {
                        MessageBox.Show("Fill with positive number.");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Fill with positive number.");
                    return;
                }
            }
            catch
            {
                MessageBox.Show("Fill with positive number.");
                return;
            }
            //check if there is not to many points relative to exam grade(in total)
            if (pointToGrade > exam.Grade)
            {
                MessageBox.Show("To many points for the exam: " + pointToGrade.ToString());
                return;
            }

            //question.IsImage = (bool)IsImageQuestion.IsChecked; //need to hendle some point, maybe dont need it

            if ((bool)IsCorrectAnswer1.IsChecked)
            {
                question.Answer1 = Answer1TXT.Text;
                question.CorrectAnswer = 1;
                is_pressed = true;
            }
            else
            {
                question.Answer1 = Answer1TXT.Text;
            }
            if ((bool)IsCorrectAnswer2.IsChecked)
            {
                if (is_pressed) MessageBox.Show("You can choose only one correct answer.");
                question.Answer2 = Answer2TXT.Text;
                question.CorrectAnswer = 2;
                is_pressed = true;
            }
            else
            {
                question.Answer2 = Answer2TXT.Text;
            }
            if ((bool)IsCorrectAnswer3.IsChecked)
            {
                if (is_pressed) MessageBox.Show("You can choose only one correct answer.");
                question.Answer3 = Answer3TXT.Text;
                question.CorrectAnswer = 3;
                is_pressed = true;
            }
            else
            {
                question.Answer3 = Answer3TXT.Text;
            }
            if ((bool)IsCorrectAnswer4.IsChecked)
            {
                if (is_pressed) MessageBox.Show("You can choose only one correct answer.");
                question.Answer4 = Answer4TXT.Text;
                question.CorrectAnswer = 4;
                is_pressed = true;
            }
            else
            {
                question.Answer4 = Answer4TXT.Text;
            }
            if (question.Answer1 == "" || question.Answer2 == "" || question.Answer3 == "" || question.Answer4 == "" )
            {
                MessageBox.Show("Fill all answers.");
                return;
            }
            if (!is_pressed)
            {
                MessageBox.Show("You need to choose correct answer.");
                return;
            }
            question.ExamId = exam.Id;
            question.Id = Guid.NewGuid().ToString();
            question.QuestionNumber = qNumber;
            qNumber++;
            pointToGrade += question.Weight;

            _questions.Add(question);
            this.QuestionsLST.ItemsSource = _questions;
            this.QuestionsLST.Items.Refresh();

            QuestionTXT.Text = "";

            QuestionWeight.Text = "";

            Answer1TXT.Text = "";

            Answer2TXT.Text = "";

            Answer3TXT.Text = "";

            Answer4TXT.Text = "";

            imgData = null;

            imgLbl.Visibility = Visibility.Collapsed;
            QuestionTXT.Visibility = Visibility.Visible;

            switch (question.CorrectAnswer)
            {
                case 1:
                    IsCorrectAnswer1.IsChecked = false;
                    break;
                case 2:
                    IsCorrectAnswer2.IsChecked = false;
                    break;
                case 3:
                    IsCorrectAnswer3.IsChecked = false;
                    break;
                case 4:
                    IsCorrectAnswer4.IsChecked = false;
                    break;
                default:
                    break;
            }
        }
        //QuestionsLST_SelectionChanged: when item from list box is selected, show details of the chosen question
        private void QuestionsLST_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.QuestionsLST.SelectedItem is Question q)
            {

                QuestionTXT.Text = q.QuestionContent;
                if (q.IsImage)
                {
                    ShowImageQuestion(q.ImageData);
                }
                else
                {
                    imgLbl.Visibility = Visibility.Collapsed;
                    QuestionTXT.Visibility = Visibility.Visible;
                }

                QuestionWeight.Text = q.Weight.ToString();

                Answer1TXT.Text = q.Answer1;

                Answer2TXT.Text = q.Answer2;

                Answer3TXT.Text = q.Answer3;

                Answer4TXT.Text = q.Answer4;
                switch (q.CorrectAnswer)
                {
                    case 1:
                        IsCorrectAnswer1.IsChecked = true;
                        IsCorrectAnswer2.IsChecked = false;
                        IsCorrectAnswer3.IsChecked = false;
                        IsCorrectAnswer4.IsChecked = false;
                        break;
                    case 2:
                        IsCorrectAnswer1.IsChecked = false;
                        IsCorrectAnswer2.IsChecked = true;
                        IsCorrectAnswer3.IsChecked = false;
                        IsCorrectAnswer4.IsChecked = false;
                        break;
                    case 3:
                        IsCorrectAnswer1.IsChecked = false;
                        IsCorrectAnswer2.IsChecked = false;
                        IsCorrectAnswer3.IsChecked = true;
                        IsCorrectAnswer4.IsChecked = false;
                        break;
                    case 4:
                        IsCorrectAnswer1.IsChecked = false;
                        IsCorrectAnswer2.IsChecked = false;
                        IsCorrectAnswer3.IsChecked = false;
                        IsCorrectAnswer4.IsChecked = true;
                        break;
                    default:
                        break;
                }
            }
        }
        //MixAnswers: mixing the order of the ansewrs
        private void MixAnswers(Question question)
        {
            //mixing answers
            List<string> answersList = new List<string>();
            answersList.Add(question.Answer1);
            answersList.Add(question.Answer2);
            answersList.Add(question.Answer3);
            answersList.Add(question.Answer4);
            string correctAnswer = "";
            switch (question.CorrectAnswer)
            {
                case 1:
                    correctAnswer = question.Answer1;
                    break;
                case 2:
                    correctAnswer = question.Answer2;
                    break;
                case 3:
                    correctAnswer = question.Answer3;
                    break;
                case 4:
                    correctAnswer = question.Answer4;
                    break;
                default:
                    break;
            }
            Random rnd = new Random();
            answersList = answersList.OrderBy(x => rnd.Next()).ToList();
            if (answersList[0] == correctAnswer)
            {
                question.Answer1 = correctAnswer;
                question.CorrectAnswer = 1;
            }else question.Answer1 = answersList[0];
            if (answersList[1] == correctAnswer)
            {
                question.Answer2 = correctAnswer;
                question.CorrectAnswer = 2;
            }
            else question.Answer2 = answersList[1];
            if (answersList[2] == correctAnswer)
            {
                question.Answer3 = correctAnswer;
                question.CorrectAnswer = 3;
            }
            else question.Answer3 = answersList[2];
            if (answersList[3] == correctAnswer)
            {
                question.Answer4 = correctAnswer;
                question.CorrectAnswer = 4;
            }
            else question.Answer4 = answersList[3];
            return;
        }
        //SaveQtBTN_Click: save questions in database, if the exam exist - PUT, else POST
        private async void SaveQtBTN_Click(object sender, RoutedEventArgs e)
        
        {
            if(_questions.Count == 0)
            {
                MessageBox.Show("Exam should contain at least one question.");
                return;
            }
            //when questions are ready to be save and be updated/added on DB
            if(pointToGrade != exam.Grade)
            {
                MessageBox.Show("You need that the question weight will be as the exam grade.");
                return;
            }
            if (exam.IsRandomize)
            {
                //optional to devide randomize for questions and answers seperated
                //mixing exam questions
                Random rnd = new Random();
                _questions = _questions.OrderBy(x => rnd.Next()).ToList();
                int i = 1;
                foreach (Question question in _questions)
                {
                    question.QuestionNumber = i;
                    i++;
                }
                
            }
            if (isAnswerRand)
            {
                foreach (var q in _questions)
                {
                    MixAnswers(q);
                }
            }
            foreach (var item in _questions)
            {
                var json = JsonConvert.SerializeObject(item);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.GetAsync(url + "/" + item.Id);
                var result = response.IsSuccessStatusCode;
                if(result == false)
                {
                    response = await client.PostAsync(url, data);
                }
                else
                {
                    response = await client.PutAsync(url + "/" + item.Id,data);
                }
                result = response.IsSuccessStatusCode;
                if (!result)
                {
                    MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
                    return;
                }

            }
            // need to close window
            this.Close();
        }
        //UpdateBtn_Click: if question exist and you want to change it, update function
        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.QuestionsLST.SelectedItem is Question question) { 
                bool is_pressed = false;

                question.QuestionContent = QuestionTXT.Text;
                if (question.QuestionContent != "" || imgData != null)
                {
                    if (imgData != null)
                    {
                        question.ImageData = imgData;
                        question.QuestionContent = filePath;
                        question.IsImage = true;
                    }
                    else
                    {
                        question.IsImage = false;
                        question.ImageData = new byte[] { 0x00 };
                    }
                }
                else
                {
                    MessageBox.Show("Fill question content.");
                    return;
                }
                pointToGrade -= question.Weight; //recalculate the weight of the question
                try
                {
                    question.Weight = float.Parse(QuestionWeight.Text);
                }
                catch
                {
                    MessageBox.Show("Fill with positive number");
                    return;
                }
               
                if (question.Weight <= 0)
                {
                    MessageBox.Show("Fill with positive number.");
                    return;
                }
                
                //to activate
                if (pointToGrade > exam.Grade)
                {
                    MessageBox.Show("To many points for the exam.");
                    return;
                }
                //need to get to the grade himself
                

                if ((bool)IsCorrectAnswer1.IsChecked)
                {
                    question.Answer1 = Answer1TXT.Text;
                    question.CorrectAnswer = 1;
                    is_pressed = true;
                }
                else
                {
                    question.Answer1 = Answer1TXT.Text;
                }
                if ((bool)IsCorrectAnswer2.IsChecked)
                {
                    if (is_pressed) MessageBox.Show("You can choose only one correct answer.");
                    question.Answer2 = Answer2TXT.Text;
                    question.CorrectAnswer = 2;
                    is_pressed = true;
                }
                else
                {
                    question.Answer2 = Answer2TXT.Text;
                }
                if ((bool)IsCorrectAnswer3.IsChecked)
                {
                    if (is_pressed) MessageBox.Show("You can choose only one correct answer.");
                    question.Answer3 = Answer3TXT.Text;
                    question.CorrectAnswer = 3;
                    is_pressed = true;
                }
                else
                {
                    question.Answer3 = Answer3TXT.Text;
                }
                if ((bool)IsCorrectAnswer4.IsChecked)
                {
                    if (is_pressed) MessageBox.Show("You can choose only one correct answer.");
                    question.Answer4 = Answer4TXT.Text;
                    question.CorrectAnswer = 4;
                    is_pressed = true;
                }
                else
                {
                    question.Answer4 = Answer4TXT.Text;
                }
                if (question.Answer1 == "" || question.Answer2 == "" || question.Answer3 == "" || question.Answer4 == "")
                {
                    MessageBox.Show("Fill all answers.");
                    return;
                }
                if (!is_pressed)
                {
                    MessageBox.Show("You need to choose correct answer.");
                    return;
                }
                pointToGrade += question.Weight;
                this.QuestionsLST.Items.Refresh();

                QuestionTXT.Text = "";

                QuestionWeight.Text = "";

                Answer1TXT.Text = "";

                Answer2TXT.Text = "";

                Answer3TXT.Text = "";

                Answer4TXT.Text = "";

                imgData = null;

                imgLbl.Visibility = Visibility.Collapsed;
                QuestionTXT.Visibility = Visibility.Visible;

                switch (question.CorrectAnswer)
                {
                    case 1:
                        IsCorrectAnswer1.IsChecked = false;
                        break;
                    case 2:
                        IsCorrectAnswer2.IsChecked = false;
                        break;
                    case 3:
                        IsCorrectAnswer3.IsChecked = false;
                        break;
                    case 4:
                        IsCorrectAnswer4.IsChecked = false;
                        break;
                    default:
                        break;
                }
            }

        }

        private void IsCorrectAnswer1_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void IsCorrectAnswer2_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void IsCorrectAnswer3_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void IsCorrectAnswer4_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void imgBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;
                imgData = File.ReadAllBytes(filePath);
                ShowImageQuestion(imgData);
                // Use HTTP Client to send image data to the ASP.NET API for storage in the SQL Server database.
            }
        }

        private void ShowImageQuestion(byte[] image)
        {
            BitmapImage bitmapImage = new BitmapImage();
            using (MemoryStream memory = new MemoryStream(image))
            {
                memory.Position = 0;
                bitmapImage.BeginInit();
                bitmapImage.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.UriSource = null;
                bitmapImage.StreamSource = memory;
                bitmapImage.DecodePixelHeight = 40;
                bitmapImage.DecodePixelWidth = 230;
                bitmapImage.EndInit();
            }

            imageShow.Source = bitmapImage;
            imageShow.Width = 230;
            imageShow.Height = 40;
            imgLbl.Content = imageShow;
            imgLbl.Visibility = Visibility.Visible;
            QuestionTXT.Visibility = Visibility.Collapsed;
        }
    }
}
