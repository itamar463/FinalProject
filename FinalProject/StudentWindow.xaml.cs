using FinalProject.Demos.Objects;
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
    /// <summary>
    /// Interaction logic for StudentWindow.xaml
    /// </summary>
    public partial class StudentWindow : Window
    {
        private StudentsRepository repo;
        private HttpClient client;
        private Student stud;
        private string url = "https://localhost:7277/api/Exams";
        private List<Exam> exams;
        public StudentWindow()
        {
            InitializeComponent();
        }

        private async void GetExams()
        {
            var response = client?.GetAsync(url).Result;
            string? dataString = response?.Content.ReadAsStringAsync().Result;
            this.exams = JsonSerializer.Deserialize<List<Exam>>(dataString);
            
        }  
        public StudentWindow(StudentsRepository repo, Student s)
        {
            InitializeComponent();
            this.repo = repo;
            this.stud = s;
            StudentLbl.Content += stud.Name;
            client = new HttpClient();
            GetExams();
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
                            ExamStudentWindow w = new ExamStudentWindow(stud,exam);
                            w.Show();
                            StudentExamTXT.Text = "";
                            return;
                        }
                        
                    }
                }
                MessageBox.Show("Exam didn't found.");
            }
        }
    }
}
