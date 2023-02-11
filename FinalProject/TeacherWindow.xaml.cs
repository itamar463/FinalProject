using FinalProject.Demos.Objects;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;


namespace FinalProject.Demos
{
    
    public partial class TeacherWindow : Window
    {
        private TeachersRepository repo;

        private Teacher teach;

        private Exam exam; //for a new exam be to created

        private List<Exam> teacherExams;

        private List<ExamData>? examsInfo;

        private HttpClient client;

        private string url = "https://localhost:7277/api/Exams";

        // The teacher window will display all of his exams and their statistics (if they were already solved by students) 
        // also, it will allow the addition of new exams, the update of existing ones and also their removal
     
        public TeacherWindow()
        {
            InitializeComponent();
        }

        private void getExamData()
        {
            //loading the data of already solved exams 
            var response = client?.GetAsync("https://localhost:7277/api/ExamDatas").Result;
            string? dataString = response?.Content.ReadAsStringAsync().Result;
            examsInfo = JsonSerializer.Deserialize<List<ExamData>>(dataString);
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
                    if(!teacherExams.Contains(item)) teacherExams.Add(item);
                    if(!ExamsCombo.Items.Contains(item.Name)) ExamsCombo.Items.Add(item.Name);
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
            examsInfo = new List<ExamData>();
            GetExams();
            getExamData();
            
            
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

        
        // will open the examDetailsWindow with no filled textboxes
        private void AddExamBTN_Click(object sender, RoutedEventArgs e)
        {
            //adding new exam
            ExamDetailsWindow w = new ExamDetailsWindow(teach);
            w.ShowDialog();
            GetExams();
        }

        //will remove all questions of a given exam (used when an exam is being deleted)
        private async void DeleteQuestions()
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
                        var response_del = await client.DeleteAsync("https://localhost:7277/api/Questions" + "/" + item.Id);
                        var result = response_del.IsSuccessStatusCode;
                        if (!result)
                        {
                            MessageBox.Show("Error Code" + response_del.StatusCode + " : Message - " + response_del.ReasonPhrase);
                        }
                    }
                }
            }
        }

        //will remove an exam's statistics (will be used when deleting an exam and if it was already solved)
        private async void DeleteExamData()
        {
            var response = client?.GetAsync("https://localhost:7277/api/ExamDatas").Result;
            string? dataString = response?.Content.ReadAsStringAsync().Result;
            List<ExamData>? allData = JsonSerializer.Deserialize<List<ExamData>>(dataString);
            if (allData != null)
            {
                foreach (var item in allData)
                {
                    if (item.ExamId == exam.Id)
                    {
                        var response_del = await client.DeleteAsync("https://localhost:7277/api/ExamDatas" + "/" + item.Id);
                        var result = response_del.IsSuccessStatusCode;
                        if (!result)
                        {
                            MessageBox.Show("Error Code" + response_del.StatusCode + " : Message - " + response_del.ReasonPhrase);
                        }
                    }
                }
            }
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
                DeleteQuestions();
                DeleteExamData();
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

        // will handle moving between tests using the exams listbox
        private void ExamsCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //TODO: need to finish not ready! - Pretty sure we're done here, but keeping this note to make sure with Itush
            Statlbl.Content = "";
            submitLbl.Content = "";
            maxGradeLbl.Content = "";
            minGradeLbl.Content = "";
            aveGradeLbl.Content = "";
            dataGrid.Visibility = Visibility.Collapsed;
            int subAmount = 0;
            float maxG=0, minG=200, ave=0;
            StudentsData sData = new StudentsData();
            GetQuestionsData(sData);
            Exam? examStat = teacherExams.Find(i => i.Name == ExamsCombo.SelectedItem);
            if(examStat != null)
            {
                Statlbl.Content = "Exam: " + examStat.Name;
                foreach (var item in examsInfo)
                {
                    if(item.ExamId == examStat.Id)
                    {
                        if (minG > item.Grade) minG = item.Grade;
                        if (maxG < item.Grade) maxG = item.Grade;
                        ave += item.Grade;
                        subAmount++;
                    }
                }
                submitLbl.Content += "Submits amount: " + subAmount.ToString();
                maxGradeLbl.Content += "Max grade: " + maxG.ToString();
                
                ave /= subAmount;
                if(subAmount != 0)
                {
                    aveGradeLbl.Content += "Average grade: " + ave.ToString();
                    minGradeLbl.Content += "Min grade: " + minG.ToString();
                }
                else
                {
                    aveGradeLbl.Content += "Average grade: 0";
                    minGradeLbl.Content += "Min grade: 0";
                }
                dataGrid.Visibility = Visibility.Visible;
                dataGrid.ItemsSource = sData.Grades;
            }
        }

        // will get an exam's statistics from the DB and display them
        private void GetQuestionsData(StudentsData data)
        {
            Exam? examStat = teacherExams.Find(i => i.Name == ExamsCombo.SelectedItem);
            if (examStat != null)
            {
                data.StudentsAnswers = new Dictionary<string, List<bool>>();
                data.Grades = new Dictionary<string, string>();
                Statlbl.Content = "Exam: " + examStat.Name;
                foreach (var item in examsInfo)
                {
                    if (item.ExamId == examStat.Id)
                    {
                        data.Grades.Add(item.StudentName, item.Grade.ToString());
                        List<string> questions = item.QuestionDetails.Split("***").ToList();
                        foreach (var question in questions)
                        {
                            List<string> details = question.Split("^^^").ToList();

                            if (!data.StudentsAnswers.ContainsKey(item.StudentName))
                            {
                                data.StudentsAnswers.Add(item.StudentName, new List<bool>());
                                if (details[1] == details[2]) data.StudentsAnswers[item.StudentName].Add(true);
                                else data.StudentsAnswers[item.StudentName].Add(false);
                            }
                            else
                            {
                                if (details[1] == details[2]) data.StudentsAnswers[item.StudentName].Add(true);
                                else data.StudentsAnswers[item.StudentName].Add(false);
                            }
                        }
                    }
                }
            }
        }

        //StudentsData is a sub-class that holds on the details of an answered exam to calcuate the statistics later
        public class StudentsData
        {
            public Dictionary<string,string> Grades { get; set; }

            
            public Dictionary<string,List<bool>> StudentsAnswers { get; set; }


        }
    }
}
