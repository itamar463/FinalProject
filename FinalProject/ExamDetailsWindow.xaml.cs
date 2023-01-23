using FinalProject.Demos.Objects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace FinalProject.Demos
{
    public partial class ExamDetailsWindow : Window
    {
        private Teacher teacher;

        private Exam exam;

        private HttpClient client;

        private bool isToUpdate = false;

        private string url = "https://localhost:7277/api/Exams";
        public ExamDetailsWindow(Teacher t)
        {
            //adding new exam
            InitializeComponent();
            this.teacher = t;
            exam = new Exam();
            client = new HttpClient();
            welcomeLbl.Content += teacher.Name;
        }
        public ExamDetailsWindow(Teacher t,Exam exam)
        {
            //updating exam
            InitializeComponent();
            this.teacher = t;
            this.exam = exam;
            client = new HttpClient();
            welcomeLbl.Content += teacher.Name;
            isToUpdate = true;
            ExamToUpdate();
        }

        void ExamToUpdate()
        {
            //fill fields from the exam to update
            examNameTxt.Text = exam.Name;
            examTimeTxt.Text = exam.Totaltime.ToString();
            examMaxGradeTxt.Text = exam.Grade.ToString();
            isRandomCheck.IsChecked = exam.IsRandomize;
            DateStart.SelectedDate = exam.StratDate;
            DateEnd.SelectedDate = exam.EndDate;
            
            
        }
        private async void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            //add exam going to add questions
            exam.Name = examNameTxt.Text;
            if(exam.Name == "")
            {
                MessageBox.Show("You have to give a name for the exam.");
                return;
            }
            exam.TeacherId = teacher.Id;
            if(examTimeTxt.Text != "" && examMaxGradeTxt.Text != "")
            {
                try
                {
                    exam.Totaltime = float.Parse(examTimeTxt.Text);
                    if (exam.Totaltime < 0.5)
                    {
                        MessageBox.Show("Exam need to be at least half hour long.");
                        return;
                    }
                    exam.Grade = int.Parse(examMaxGradeTxt.Text);
                    if (exam.Grade < 10 || exam.Grade > 200)
                    {
                        MessageBox.Show("Exam need to be between 10 to 200 in max grade.");
                        return;
                    }
                }
                catch
                {
                    MessageBox.Show("Enter valid number.");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Fill time and grade fields.");
                return;
            }
            
            exam.IsRandomize = (bool)isRandomCheck.IsChecked;
            if(DateStart.SelectedDate != null && DateEnd.SelectedDate != null)
            {
                exam.StratDate = DateStart.SelectedDate.Value;
                exam.EndDate = DateEnd.SelectedDate.Value;
            }
            else
            {
                MessageBox.Show("Choose dates to activate exam.");
                return;
            }
            var json = JsonConvert.SerializeObject(exam);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            //if new exam need to POST if to update PUT
            if (!isToUpdate)
            {
                var response = await client.PostAsync(url, data);
                var result = response.IsSuccessStatusCode;
                if (!result)
                {
                    MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
                    return;
                }
            }
            else
            {
                var response = await client.PutAsync(url + "/" + exam.Id, data);
                var result = response.IsSuccessStatusCode;
                if (!result)
                {
                    MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
                    return;
                }
            }
            
  
            AddExamWindow w = new AddExamWindow(exam);
            w.Show();
        }
    }
}
