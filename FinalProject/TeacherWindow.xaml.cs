using FinalProject.Demos.Objects;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Runtime.Intrinsics.X86;
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

        private List<ExamData>? examsInfo;

        private HttpClient client;

        private string url = "https://localhost:7277/api/Exams";
        public TeacherWindow()
        {
            InitializeComponent();
            
        }

        private void getExamData()
        {
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
            //need to finish not ready!
            //ExamsCombo.Items
            //List<ExamData> data = new List<ExamData>();
            submitLbl.Content = "";
            maxGradeLbl.Content = "";
            minGradeLbl.Content = "";
            aveGradeLbl.Content = "";
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

                // var check = sData.Grades.SelectMany(x => x.Value.Select(y => new { Key = x.Key, Value = y }));
                //dataGrid.ItemsSource = sData.Grades.SelectMany(x => x.Value.Select(y => new { Key = x.Key, Value = y }));
                dataGrid.ItemsSource = sData.Grades;
  
                
                //foreach (var item in sData.StudentsAnswers)
                //{
                //    int i = 1;
                //    foreach (var answer in item.Value)
                //    {
                //        string Val = answer.ToString();
                //        DataGridTextColumn column = new DataGridTextColumn();
                //        column.Header = "Question " + i.ToString();
                //        column.Binding = new Binding("Val");
                //        dataGrid.Columns.Add(column);
                //        i++;
                //    }
                //}
                //if (sData != null) dataGrid.ItemsSource = sData.Grades;

                //need to iterate the questions string in each item in data
                //DataGridTextColumn column = new DataGridTextColumn();
                //column.Header = "Column Name";
                //column.Binding = new Binding("PropertyName");
                //dataGrid.Columns.Add(column);

            }
        }
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
        public class StudentsData
        {
            //public string ExamName { get; set; }
            public Dictionary<string,string> Grades { get; set; }

            
            public Dictionary<string,List<bool>> StudentsAnswers { get; set; }


        }
    }
}
