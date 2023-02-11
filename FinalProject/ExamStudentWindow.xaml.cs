using FinalProject.Demos.Objects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Image = System.Windows.Controls.Image;


namespace FinalProject.Demos
{
    // This window will show the test on the student's end,
    // i.e when a student takes a test, this will be handled by this window
    public partial class ExamStudentWindow : Window
    {
        private Student student;
        private Exam exam;
        HttpClient client;
        private List<Question> questions;
        private static int curr_question = 0;
        private Dictionary<string, List<bool>> answers; //will store each questions and the value of each answer entered by the student (true if the student chose the answer as the correct one, all others will be defaultly false)
        private DispatcherTimer timer; 
        private TimeSpan remainingTime;// will display at each second the time left for the exam to end
        private ExamData? examData;
        private static float percentage = 0;
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

        // will handle the job of the timer. When the time ends - the window will simply close
        // also, we put here a listener to the exam's progress bar for it to be updated accordingly to the exam's progress
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
                percentage = 0;
                int index = 0;
                foreach (KeyValuePair<string, List<bool>> item in answers)
                {
                    if(item.Value.FindAll(x => x == true).Count >=1)
                    {
                        percentage += questions[index].Weight;
                    }
                    index++;
                }
                progBar.Value = percentage;

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
        
        //Will handle image-type questions and have them shown properly
        private void ImageContent(Question q)
        {
            Image imageControl = new Image();
            BitmapImage bitmapimage = new BitmapImage();
            using (MemoryStream memory = new MemoryStream(q.ImageData))
            {
                memory.Position = 0;
                bitmapimage.BeginInit();
                bitmapimage.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.UriSource = null;
                bitmapimage.StreamSource = memory;
                bitmapimage.DecodePixelHeight = 40;
                bitmapimage.DecodePixelWidth = 250;
                bitmapimage.EndInit();
            }

            imageControl.Source = bitmapimage;
            imageControl.Width = 250;
            imageControl.Height = 40;
            QuestionImgLbl.Content = imageControl;
            QuestionLbl.Content = q.QuestionContent;
            QuestionLbl.Visibility = Visibility.Collapsed;
            QuestionImgLbl.Visibility = Visibility.Visible;
        }
        
        //Will initialize the time and load all the data to it's place on the window
        private void startExam()
        {
            if (questions.Count > 0)
            {
                QuestionNumberLbl.Content = "Question Number: " +  questions[0].QuestionNumber.ToString();
                if (questions[0].IsImage)
                {
                    ImageContent(questions[0]);
                }
                else
                {
                    QuestionLbl.Content = questions[0].QuestionContent;
                    QuestionLbl.Visibility = Visibility.Visible;
                    QuestionImgLbl.Visibility = Visibility.Collapsed;
                }
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
        
        // will load already answered questions with the student's choice when going back to answered questions in the test
        private void CheckedAnswers(string q_content)
        {
            List<bool> checkBoxes = answers[q_content];
            if (checkBoxes[0] == true)
            {
                IsCorrectAnswer1.IsChecked = true;
                
            }
            else IsCorrectAnswer1.IsChecked = false;
            if (checkBoxes[1] == true)
            {
                IsCorrectAnswer2.IsChecked = true;
                
            }
            else IsCorrectAnswer2.IsChecked = false;
            if (checkBoxes[2] == true)
            {
                IsCorrectAnswer3.IsChecked = true;
                
            }
            else IsCorrectAnswer3.IsChecked = false;
            if (checkBoxes[3] == true)
            {
                IsCorrectAnswer4.IsChecked = true;
                
            }
            else IsCorrectAnswer4.IsChecked = false;
        }
        
        //Calculate a student's final grade in the exam
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
                    //if the student didn't answer the question, will give 0 as picked answer
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
            // When the finish exam button is pressed, we'll get here -
            // If not all the questions were answered, we will make sure with the student whether he'd really like to exit the test
            foreach (KeyValuePair<string, List<bool>> item in answers)
            {
                if (item.Value.FindAll(x => x == true).Count == 0)
                {
                    if (MessageBox.Show("Are you sure you want to finish?", "Exam is not finished", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                    {
                        return;
                    }
                    else
                    {
                        SubmitExam();
                        this.Close();
                    }

                }

            }
            SubmitExam();
            this.Close();
        }
        // will move to the exam's previous question and load it's data
        private void PrevBtn_Click(object sender, RoutedEventArgs e)
        {
            curr_question--;
            if (curr_question < 0) curr_question += questions.Count;
            int index = curr_question % questions.Count;
            
            QuestionNumberLbl.Content = "Question Number: " + questions[index].QuestionNumber.ToString();
            if (questions[index].IsImage) ImageContent(questions[index]);
            else
            {
                QuestionLbl.Content = questions[index].QuestionContent;
                QuestionLbl.Visibility = Visibility.Visible;
                QuestionImgLbl.Visibility = Visibility.Collapsed;
            }
            Answer1Lbl.Content = questions[index].Answer1;
            Answer2Lbl.Content = questions[index].Answer2;
            Answer3Lbl.Content = questions[index].Answer3;
            Answer4Lbl.Content = questions[index].Answer4;
            
            CheckedAnswers(QuestionLbl.Content.ToString());
            
        }

        // will move to the exam's next question and load it's data
        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            curr_question++;
            int index = curr_question % questions.Count;
            
            
            QuestionNumberLbl.Content = "Question Number: " + questions[index].QuestionNumber.ToString();
            if (questions[index].IsImage) ImageContent(questions[index]);
            else
            {
                QuestionLbl.Content = questions[index].QuestionContent;
                QuestionLbl.Visibility = Visibility.Visible;
                QuestionImgLbl.Visibility = Visibility.Collapsed;
            }
            Answer1Lbl.Content = questions[index].Answer1;
            Answer2Lbl.Content = questions[index].Answer2;
            Answer3Lbl.Content = questions[index].Answer3;
            Answer4Lbl.Content = questions[index].Answer4;
            
            CheckedAnswers(QuestionLbl.Content.ToString());
            
        }

        // moving between questions in the time of the exam using the listbox (updating the questions data accordingly)
        private void QuestionsLST_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // questions list box
            if (this.QuestionsLST.SelectedItem is Question q)
            {
                
                int index = QuestionsLST.SelectedIndex;
                curr_question = index;
                QuestionNumberLbl.Content = "Question Number: " + questions[index].QuestionNumber.ToString();
                if (questions[index].IsImage) ImageContent(questions[index]);
                else
                {
                    QuestionLbl.Content = questions[index].QuestionContent;
                    QuestionLbl.Visibility = Visibility.Visible;
                    QuestionImgLbl.Visibility = Visibility.Collapsed;
                }
                Answer1Lbl.Content = questions[index].Answer1;
                Answer2Lbl.Content = questions[index].Answer2;
                Answer3Lbl.Content = questions[index].Answer3;
                Answer4Lbl.Content = questions[index].Answer4;
                
                CheckedAnswers(QuestionLbl.Content.ToString());
                
            }
        }
        
        // if the 1st checkbox was checked - 
        private void IsCorrectAnswer1_Checked(object sender, RoutedEventArgs e)
        {
            IsCorrectAnswer1.Unchecked += IsCorrectAnswer1_Unchecked;
            if (QuestionLbl.Content != "")
            {
                if ((bool)IsCorrectAnswer1.IsChecked)
                {
                    answers[QuestionLbl.Content.ToString()][0] = true;
                    
                }

            }
            
        }
        // if the 1st checkbox was unchecked - 

        private void IsCorrectAnswer1_Unchecked(object sender, RoutedEventArgs e)
        {
            if (QuestionLbl.Content != "")
            {
                if (!(bool)IsCorrectAnswer1.IsChecked) answers[QuestionLbl.Content.ToString()][0] = false;
                
                
            }

        }
        
        // if the 2nd checkbox was checked - 
        private void IsCorrectAnswer2_Checked(object sender, RoutedEventArgs e)
        {
            IsCorrectAnswer2.Unchecked += IsCorrectAnswer2_Unchecked;
            if (QuestionLbl.Content != "")
            {
                if ((bool)IsCorrectAnswer2.IsChecked)
                {
                    answers[QuestionLbl.Content.ToString()][1] = true;
                    
                }
                
            }
        }

        // if the 2nd checkbox was unchecked - 
        private void IsCorrectAnswer2_Unchecked(object sender, RoutedEventArgs e)
        {
            if (QuestionLbl.Content != "")
            {
                if (!(bool)IsCorrectAnswer2.IsChecked) answers[QuestionLbl.Content.ToString()][1] = false;
               
            }

        }

        // if the 3rd checkbox was checked - 
        private void IsCorrectAnswer3_Checked(object sender, RoutedEventArgs e)
        {

            IsCorrectAnswer3.Unchecked += IsCorrectAnswer3_Unchecked;
            if (QuestionLbl.Content != "")
            {
                if ((bool)IsCorrectAnswer3.IsChecked)
                {
                    answers[QuestionLbl.Content.ToString()][2] = true;
                    

                }
                
            }
        }

        // if the 3rd checkbox was unchecked - 
        private void IsCorrectAnswer3_Unchecked(object sender, RoutedEventArgs e)
        {
            if (QuestionLbl.Content != "")
            {
                if (!(bool)IsCorrectAnswer3.IsChecked) answers[QuestionLbl.Content.ToString()][2] = false;
                
                
            }

        }

        // if the 4th checkbox was checked - 
        private void IsCorrectAnswer4_Checked(object sender, RoutedEventArgs e)
        {

            IsCorrectAnswer4.Unchecked += IsCorrectAnswer4_Unchecked;
            if (QuestionLbl.Content != "")
            {
                if ((bool)IsCorrectAnswer4.IsChecked)
                {
                    answers[QuestionLbl.Content.ToString()][3] = true;
                    
                }
                
            }
        }

        // if the 4th checkbox was unchecked - 
        private void IsCorrectAnswer4_Unchecked(object sender, RoutedEventArgs e)
        {
            if (QuestionLbl.Content != "")
            {
                if (!(bool)IsCorrectAnswer4.IsChecked) answers[QuestionLbl.Content.ToString()][3] = false;
                
                

            }

        }

        //updating the progress bar value
        private void progBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            progBar.Value = (int)e.NewValue;
        }
    }
}
