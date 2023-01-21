using FinalProject.Demos.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// <summary>
    /// Interaction logic for ExamDetailsWindow.xaml
    /// </summary>
    public partial class ExamDetailsWindow : Window
    {
        private Teacher teacher;

        private Exam exam;
        public ExamDetailsWindow(Teacher t)
        {
            InitializeComponent();
            this.teacher = t;
            exam = new Exam();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            exam.Name = examNameTxt.Text;
            exam.Teacher = teacher;
            exam.Totaltime = float.Parse(examTimeTxt.Text);
            exam.Grade = int.Parse(examMaxGradeTxt.Text);
            exam.IsRandomize = (bool)isRandomCheck.IsChecked;
            exam.StratDate = DateStart.SelectedDate.Value;
            exam.EndDate = DateEnd.SelectedDate.Value;
            //need to post exam
        }
    }
}
