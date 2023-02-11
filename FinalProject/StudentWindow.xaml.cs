using FinalProject.Demos.Objects;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Windows;

namespace FinalProject.Demos
{

    public partial class StudentWindow : Window
    {
        //This window will be the one a student will be directed to after logging in
        // in here we will load all the exam data and allow the student to enter an exam key and test it (if the exam is at all relevant) 
        private HttpClient client;
        private Student stud;
        private string url = "https://localhost:7277/api/Exams";
        private List<Exam> exams;
        public StudentWindow()
        {
            InitializeComponent();
        }

        //loading all exams
        private async void GetExams()
        {
            var response = client?.GetAsync(url).Result;
            string? dataString = response?.Content.ReadAsStringAsync().Result;
            this.exams = JsonSerializer.Deserialize<List<Exam>>(dataString);
            
        }  

        public StudentWindow(StudentsRepository repo, Student s)
        {
            InitializeComponent();
            this.stud = s;
            StudentLbl.Content += stud.Name;
            client = new HttpClient();
            GetExams();
        }

        //Will check if the exam entered by the student was already done by him 
        private bool checkIfDidExam(string examId)
        {
            var response = client?.GetAsync("https://localhost:7277/api/ExamDatas").Result;
            
            string? dataString = response?.Content.ReadAsStringAsync().Result;
            List<ExamData>? e_data = JsonSerializer.Deserialize<List<ExamData>>(dataString);
            if(e_data.Find(x => x.StudentId == stud.Id && x.ExamId == examId) != null)
            {
                return true;
            }
            return false;
        }
        private void EnterExam_Click(object sender, RoutedEventArgs e)
        {
            //this func check if the exam exist and in the date range if true: open exam Window
            DateTime today = DateTime.Now;
            string examName = StudentExamTXT.Text;
            if (examName != "")
            {
                foreach (var exam in exams)
                {
                    if(exam.Name == examName)
                    {
                        if(today < exam.StratDate)
                        {
                            MessageBox.Show("Exam isn't open yet.");
                            return;
                        }else if(today > exam.EndDate)
                        {
                            MessageBox.Show("Exam date is over.");
                            return;
                        }
                        else
                        {
                            if (!checkIfDidExam(exam.Id))
                            {
                                ExamStudentWindow w = new ExamStudentWindow(stud, exam);
                                w.Show();
                                StudentExamTXT.Text = "";
                                return;
                            }
                            else
                            {
                                MessageBox.Show("You allready did the exam!");
                                this.Close();
                                return;                          
                            }
                        }
                    }
                }
                MessageBox.Show("Exam wasn't found.");
            }
        }
    }
}
