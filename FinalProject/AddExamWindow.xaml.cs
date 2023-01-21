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

namespace FinalProject.Demos
{
    
    public partial class AddExamWindow : Window
    {
        private static float pointToGrade = 0;

        private static int qNumber = 1;

        private List<Question> _questions;

        private Exam exam;
        private HttpClient client;
        
        

        private string url = "https://localhost:7277/api/Questions";
             
    public AddExamWindow(Exam exam)
        {
            InitializeComponent();
            _questions = new List<Question>();
            this.exam = exam;
            helloLbl.Content += exam.Teacher.Name;
            client = new HttpClient();
    }

        private void RemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.QuestionsLST.SelectedItem is Question q)
            {
                

                

                //var result = await response.Content.ReadAsStringAsync();
                
                int curr_num = q.QuestionNumber;
                if(curr_num < qNumber)
                {
                    qNumber--;
                    while (curr_num < qNumber)
                    {
                        _questions[curr_num].QuestionNumber--;
                        //var json = JsonConvert.SerializeObject(_questions[curr_num]);
                        //var data = new StringContent(json, Encoding.UTF8, "application/json");
                        //var response = await client.PutAsync(url + "/" + _questions[curr_num].Id,data);
                        curr_num++;
                    }
                    
                }
                //var response_del = await client.DeleteAsync(url + "/" + q.Id);
                //var result = response_del.IsSuccessStatusCode;
                //if (!result)
                //{
                    //MessageBox.Show("Error Code" + response_del.StatusCode + " : Message - " + response_del.ReasonPhrase);
                //}
                _questions.Remove(q);
                QuestionsLST.ItemsSource = _questions;
                QuestionsLST.Items.Refresh();
                QuestionsLST.SelectedIndex = 0;
            }
        }
        
        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
        
            //create new question
            var question = new Question();
            //to check that there is correct answer
            bool is_pressed = false;

            question.QuestionContent = QuestionTXT.Text;
            if (question.QuestionContent == "")
            {
                MessageBox.Show("Fill question content.");
                return;
            }
            question.Weight = float.Parse(QuestionWeight.Text);
            if (question.Weight <= 0)
            {
                MessageBox.Show("Fill with positive number.");
                return;
            }
            else pointToGrade += question.Weight;
            //to activate
            //if(pointToGrade > exam.Grade) MessageBox.Show("To many points for the exam.");

            question.IsImage = (bool)IsImageQuestion.IsChecked; //need to hendle some point

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
                      
            _questions.Add(question);
            this.QuestionsLST.ItemsSource = _questions;
            this.QuestionsLST.Items.Refresh();

            QuestionTXT.Text = "";

            QuestionWeight.Text = "";

            Answer1TXT.Text = "";

            Answer2TXT.Text = "";

            Answer3TXT.Text = "";

            Answer4TXT.Text = "";

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

        private void QuestionsLST_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.QuestionsLST.SelectedItem is Question q)
            {

                QuestionTXT.Text = q.QuestionContent;

                QuestionWeight.Text = q.Weight.ToString();

                Answer1TXT.Text = q.Answer1;

                Answer2TXT.Text = q.Answer2;

                Answer3TXT.Text = q.Answer3;

                Answer4TXT.Text = q.Answer4;
                switch (q.CorrectAnswer)
                {
                    case 1:
                        IsCorrectAnswer1.IsChecked = true;
                        break;
                    case 2:
                        IsCorrectAnswer2.IsChecked = true;
                        break;
                    case 3:
                        IsCorrectAnswer3.IsChecked = true;
                        break;
                    case 4:
                        IsCorrectAnswer4.IsChecked = true;
                        break;
                    default:
                        break;
                }
            }
        }

        //private void RandomizeQuestions_Click(object sender, RoutedEventArgs e)
        //{

        //    Random rnd = new Random();
        //    _questions = _questions.OrderBy(x => rnd.Next()).ToList();
        //}

        
        private async void SaveQtBTN_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in _questions)
            {
                var json = JsonConvert.SerializeObject(item);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, data);
                var result = response.IsSuccessStatusCode;
                if (!result)
                {
                    MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
                    return;
                }

            }
        }

        private void IsImageQuestion_Checked(object sender, RoutedEventArgs e)
        {

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
    }
}
