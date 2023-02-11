using FinalProject.Demos.Objects;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Windows;

namespace FinalProject.Demos
{
    //This window handles the addition or update of an exam in terms of the exam details (date, time, etc) to add or update
    // (will either display an empty template to fill with the exam details,
    // or one with an exam's details available to be edited - 
    // depends on the usage of the window
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
            //clicking on the add questions button will first lead to a validation of the inital details entered for the test on this window
            // if everything is valid - will forward us to the AddExamWindow, else - messages will be displayed accordingly.
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
            bool answerRand = (bool)isRandomAnswersCheck.IsChecked;
            AddExamWindow w = new AddExamWindow(exam,answerRand);
            w.Show();
            this.Close();
        }
        
    }
}
