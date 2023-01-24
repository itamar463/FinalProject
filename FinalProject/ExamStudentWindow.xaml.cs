﻿using FinalProject.Demos.Objects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace FinalProject.Demos
{
    
    public partial class ExamStudentWindow : Window
    {
        private Student student;
        private Exam exam;
        HttpClient client;
        private List<Question> questions;
        private static int curr_question = 0;
        private Dictionary<string, List<bool>> answers;
        private DispatcherTimer timer;
        private TimeSpan remainingTime;
        private ExamData? examData;
        private static float percentage = 0;
        private static bool isChangeQuestion = false;
        

        //private bool isThereDatails = false;
        private string url = "https://localhost:7277/api/ExamDatas";
        public ExamStudentWindow()
        {
            InitializeComponent();
        }
        public ExamStudentWindow(Student student, Exam exam)
        {
            InitializeComponent();
            this.student = student;
            this.exam = exam;
            client = new HttpClient();
            examLbl.Content += exam.Name;
            questions = new List<Question>();
            answers = new Dictionary<string, List<bool>>();
            remainingTime = TimeSpan.FromHours(exam.Totaltime);
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
            GetQuestions();
            UpdateExamDetails();
            startExam();
            progBar.Value = 0;
            

        }
        

        

        private async void UpdateExamDetails()
        {
            examData = new ExamData();
            examData.StudentName = student.Name;
            examData.StudentId = student.Id;
            examData.ExamId = exam.Id;
            examData.QuestionCount = questions.Count;
        }

        void Timer_Tick(object sender, EventArgs e)
        {
            remainingTime = remainingTime.Subtract(TimeSpan.FromSeconds(1));
            if (remainingTime.TotalSeconds <= 0)
            {
                timer.Stop();
                SubmitExam();
                this.Close();
            }
            else
            {
                lblTime.Content = remainingTime.ToString(@"hh\:mm\:ss");
            }
        }
        private async void GetQuestions()
        {
            var response = client?.GetAsync("https://localhost:7277/api/Questions").Result;
            string? dataString = response?.Content.ReadAsStringAsync().Result;
            List<Question>? allQuestions = JsonSerializer.Deserialize<List<Question>>(dataString);
            if (allQuestions != null)
            {
                foreach (var item in allQuestions)
                {
                    if (item.ExamId == exam.Id)
                    {
                        questions.Add(item);
                        answers.Add(item.QuestionContent, new List<bool>(Enumerable.Repeat(false, 4)));
                        
                    }
                }
                questions = questions.OrderBy(x => x.QuestionNumber).ToList();
                
                QuestionsLST.ItemsSource = questions;
                QuestionsLST.Items.Refresh();
            }
        }
        
        private void startExam()
        {
            //Clayton Fellin
            //need timer for test remaning time
            if (questions.Count > 0)
            {
                QuestionNumberLbl.Content = "Question Number: " +  questions[0].QuestionNumber.ToString();
                QuestionLbl.Content = questions[0].QuestionContent;
                Answer1Lbl.Content = questions[0].Answer1;
                Answer2Lbl.Content = questions[0].Answer2;
                Answer3Lbl.Content = questions[0].Answer3;
                Answer4Lbl.Content = questions[0].Answer4;
            }
            else
            {
                MessageBox.Show("No questions for the test.");
                this.Close();
            }
            
        }

        private void CheckedAnswers(string q_content)
        {
            List<bool> checkBoxes = answers[q_content];
            if (checkBoxes[0] == true)
            {
                IsCorrectAnswer1.IsChecked = true;
                isChangeQuestion = true;
            }
            else IsCorrectAnswer1.IsChecked = false;
            if (checkBoxes[1] == true)
            {
                IsCorrectAnswer2.IsChecked = true;
                isChangeQuestion = true;
            }
            else IsCorrectAnswer2.IsChecked = false;
            if (checkBoxes[2] == true)
            {
                IsCorrectAnswer3.IsChecked = true;
                isChangeQuestion = true;
            }
            else IsCorrectAnswer3.IsChecked = false;
            if (checkBoxes[3] == true)
            {
                IsCorrectAnswer4.IsChecked = true;
                isChangeQuestion = true;
            }
            else IsCorrectAnswer4.IsChecked = false;
        }
        
        private void CalculateGrade()
        {
            foreach (var item in questions)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (answers[item.QuestionContent][i] == true)
                    {
                        if((i+1) == item.CorrectAnswer)
                        {
                            examData.Grade += item.Weight;
                        }
                    }
                }
            }
        }

        private async void SubmitExam()
        {
            //submit exam btn
            // will contain in one string the q name picked answer and correct answer
            // delimiter for questions : ***
            //delimiter for q name,picked answer,right answer : ^^^
            int count = 0;
            bool isAnswerd = false;
            foreach (var item in questions)
            {
                examData.QuestionDetails += item.QuestionContent + "^^^";
                for (int i = 0; i < 4; i++)
                {
                    if (answers[item.QuestionContent][i] == true)
                    {
                        examData.QuestionDetails += (i + 1).ToString() + "^^^";
                        isAnswerd = true;
                        break;
                    }
                }
                if (!isAnswerd)
                {
                    //if didn't answerd the question giving 0 as picked answer
                    examData.QuestionDetails += "0^^^";
                    
                }
                isAnswerd = false;
                if (count < (questions.Count - 1)) examData.QuestionDetails += item.CorrectAnswer.ToString() + "***";
                else examData.QuestionDetails += item.CorrectAnswer.ToString();
                count++;
            }
            CalculateGrade();
            var json = JsonConvert.SerializeObject(examData);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, data);
            var result = response.IsSuccessStatusCode;
            if (!result)
            {
                MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
                return;
            }
        }
        private async void FinishExamBTN_Click(object sender, RoutedEventArgs e)
        {
            SubmitExam();
            this.Close();
        }

        private void PrevBtn_Click(object sender, RoutedEventArgs e)
        {
            curr_question--;
            int index = curr_question % questions.Count;
            if (index < 0) index += questions.Count;
            QuestionNumberLbl.Content = "Question Number: " + questions[index].QuestionNumber.ToString();
            QuestionLbl.Content = questions[index].QuestionContent;
            Answer1Lbl.Content = questions[index].Answer1;
            Answer2Lbl.Content = questions[index].Answer2;
            Answer3Lbl.Content = questions[index].Answer3;
            Answer4Lbl.Content = questions[index].Answer4;
            isChangeQuestion = true;
            CheckedAnswers(QuestionLbl.Content.ToString());
            
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            curr_question++;
            int index = curr_question % questions.Count;
            QuestionNumberLbl.Content = "Question Number: " + questions[index].QuestionNumber.ToString();
            QuestionLbl.Content = questions[index].QuestionContent;
            Answer1Lbl.Content = questions[index].Answer1;
            Answer2Lbl.Content = questions[index].Answer2;
            Answer3Lbl.Content = questions[index].Answer3;
            Answer4Lbl.Content = questions[index].Answer4;
            isChangeQuestion = true;
            CheckedAnswers(QuestionLbl.Content.ToString());
            
        }

        private void QuestionsLST_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // questions list box
            if (this.QuestionsLST.SelectedItem is Question q)
            {
                
                int index = QuestionsLST.SelectedIndex;
                curr_question = index;
                QuestionNumberLbl.Content = "Question Number: " + questions[index].QuestionNumber.ToString();
                QuestionLbl.Content = questions[index].QuestionContent;
                Answer1Lbl.Content = questions[index].Answer1;
                Answer2Lbl.Content = questions[index].Answer2;
                Answer3Lbl.Content = questions[index].Answer3;
                Answer4Lbl.Content = questions[index].Answer4;
                isChangeQuestion = true;
                CheckedAnswers(QuestionLbl.Content.ToString());
                
            }
        }
        
        private void IsCorrectAnswer1_Checked(object sender, RoutedEventArgs e)
        {
            IsCorrectAnswer1.Unchecked += IsCorrectAnswer1_Unchecked;
            if (QuestionLbl.Content != "")
            {
                if ((bool)IsCorrectAnswer1.IsChecked)
                {
                    answers[QuestionLbl.Content.ToString()][0] = true;
                    isChangeQuestion = false;
                }
                if(answers[QuestionLbl.Content.ToString()].FindAll(x=> x==true).Count == 1)
                {
                    percentage += questions[curr_question % questions.Count].Weight;
                    progBar.Value = percentage;
                }
                

            }
            
        }
        private void IsCorrectAnswer1_Unchecked(object sender, RoutedEventArgs e)
        {
            if (QuestionLbl.Content != "")
            {
                if (!(bool)IsCorrectAnswer1.IsChecked) answers[QuestionLbl.Content.ToString()][0] = false;
                if (!isChangeQuestion && answers[QuestionLbl.Content.ToString()].FindAll(x => x == false).Count == answers[QuestionLbl.Content.ToString()].Count)
                {
                    percentage += (questions[curr_question % questions.Count].Weight * -1);
                    progBar.Value = percentage;
                    isChangeQuestion = false;
                }
                
            }

        }

        private void IsCorrectAnswer2_Checked(object sender, RoutedEventArgs e)
        {
            IsCorrectAnswer2.Unchecked += IsCorrectAnswer2_Unchecked;
            if (QuestionLbl.Content != "")
            {
                if ((bool)IsCorrectAnswer2.IsChecked)
                {
                    answers[QuestionLbl.Content.ToString()][1] = true;
                    isChangeQuestion = false;
                }
                if (answers[QuestionLbl.Content.ToString()].FindAll(x => x == true).Count == 1)
                {
                    percentage += questions[curr_question % questions.Count].Weight;
                    progBar.Value = percentage;
                }
            }
        }
        private void IsCorrectAnswer2_Unchecked(object sender, RoutedEventArgs e)
        {
            if (QuestionLbl.Content != "")
            {
                if (!(bool)IsCorrectAnswer2.IsChecked) answers[QuestionLbl.Content.ToString()][1] = false;
                if (!isChangeQuestion && answers[QuestionLbl.Content.ToString()].FindAll(x => x == false).Count == answers[QuestionLbl.Content.ToString()].Count)
                {
                    percentage += (questions[curr_question % questions.Count].Weight * -1);
                    progBar.Value = percentage;
                }
                
            }

        }

        private void IsCorrectAnswer3_Checked(object sender, RoutedEventArgs e)
        {

            IsCorrectAnswer3.Unchecked += IsCorrectAnswer3_Unchecked;
            if (QuestionLbl.Content != "")
            {
                if ((bool)IsCorrectAnswer3.IsChecked)
                {
                    answers[QuestionLbl.Content.ToString()][2] = true;
                    isChangeQuestion = false;

                }
                if (answers[QuestionLbl.Content.ToString()].FindAll(x => x == true).Count == 1)
                {
                    percentage += questions[curr_question % questions.Count].Weight;
                    progBar.Value = percentage;
                }
            }
        }
        private void IsCorrectAnswer3_Unchecked(object sender, RoutedEventArgs e)
        {
            if (QuestionLbl.Content != "")
            {
                if (!(bool)IsCorrectAnswer3.IsChecked) answers[QuestionLbl.Content.ToString()][2] = false;
                if (!isChangeQuestion && answers[QuestionLbl.Content.ToString()].FindAll(x => x == false).Count == answers[QuestionLbl.Content.ToString()].Count)
                {
                    percentage += (questions[curr_question % questions.Count].Weight * -1);
                    progBar.Value = percentage;
                    
                }
                
            }

        }
        private void IsCorrectAnswer4_Checked(object sender, RoutedEventArgs e)
        {

            IsCorrectAnswer4.Unchecked += IsCorrectAnswer4_Unchecked;
            if (QuestionLbl.Content != "")
            {
                if ((bool)IsCorrectAnswer4.IsChecked)
                {
                    answers[QuestionLbl.Content.ToString()][3] = true;
                    isChangeQuestion = false;
                }
                if (answers[QuestionLbl.Content.ToString()].FindAll(x => x == true).Count == 1)
                {
                    percentage += questions[curr_question % questions.Count].Weight;
                    progBar.Value = percentage;
                }
            }
        }
        private void IsCorrectAnswer4_Unchecked(object sender, RoutedEventArgs e)
        {
            if (QuestionLbl.Content != "")
            {
                if (!(bool)IsCorrectAnswer4.IsChecked) answers[QuestionLbl.Content.ToString()][3] = false;
                if (!isChangeQuestion && answers[QuestionLbl.Content.ToString()].FindAll(x => x == false).Count == answers[QuestionLbl.Content.ToString()].Count)
                {
                    percentage += (questions[curr_question % questions.Count].Weight * -1);
                    progBar.Value = percentage;
                    
                }
                

            }

        }

        private void progBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            progBar.Value = (int)e.NewValue;
        }
    }
}
