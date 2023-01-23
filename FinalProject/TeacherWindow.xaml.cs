using FinalProject.Demos.Objects;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

namespace FinalProject.Demos
{
    public partial class TeacherWindow : Window
    {
        private TeachersRepository repo;

        private Teacher teach;

        private Exam exam; //for new exam to create

        private List<Exam> teacherExams;

        private HttpClient client;

        private string url = "https://localhost:7277/api/Exams";
        public TeacherWindow()
        {
            InitializeComponent();
            
        }
        private void GetExams()
        {
            //add to combobox and teacher exmas list all the relative exams from DB
            var response = client?.GetAsync(url).Result;
            string? dataString = response?.Content.ReadAsStringAsync().Result;
            List<Exam>? exams = JsonSerializer.Deserialize<List<Exam>>(dataString);
            foreach (var item in exams)
            {
                if(item.TeacherId == teach.Id)
                {
                    teacherExams.Add(item);
                    ExamsCombo.Items.Add(item.Name);
                }
            }
        } 

        public TeacherWindow(TeachersRepository repo, Teacher t)
        {
            InitializeComponent();
            this.repo = repo;
            this.teach = t;
            Teacherlbl.Content += t.Name;
            client = new HttpClient();
            teacherExams = new List<Exam>();
            GetExams();
        }


        private void UpdateExamBTN_Click(object sender, RoutedEventArgs e)
        {
            //updating existing exam
            if(teacherExams.Count > 0)
            {
                foreach (var item in teacherExams)
                {
                    if (ExamsCombo.SelectedItem == item.Name)
                    {
                        exam = item;
                    }
                }
            }
            
            //send with exists teacher exam
            if(exam != null)
            {
                ExamDetailsWindow w = new ExamDetailsWindow(teach, exam);
                w.Show();
            }
            
        }

        private void AddExamBTN_Click(object sender, RoutedEventArgs e)
        {
            //adding new exam
            ExamDetailsWindow w = new ExamDetailsWindow(teach);
            w.Show();
        }

        private async void RemoveExamBTN_Click(object sender, RoutedEventArgs e)
        {
            //remove exam button
            if (teacherExams.Count > 0)
            {
                foreach (var item in teacherExams)
                {
                    if (ExamsCombo.SelectedItem == item.Name)
                    {
                        exam = item;
                    }
                }
            }

            //send with exists teacher exam
            if (exam != null)
            {
                var response_del = await client.DeleteAsync(url + "/" + exam.Id);
                var result = response_del.IsSuccessStatusCode;
                if (!result)
                {
                    MessageBox.Show("Error Code" + response_del.StatusCode + " : Message - " + response_del.ReasonPhrase);
                }
                ExamsCombo.Items.Remove(exam.Name);
                teacherExams.Remove(exam);
            }
        }

        private void ExamsCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //ExamsCombo.Items
        }
    }
}
