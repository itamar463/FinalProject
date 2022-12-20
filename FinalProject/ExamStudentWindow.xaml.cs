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

namespace FinalProject
{
    /// <summary>
    /// Interaction logic for ExamStudentWindow.xaml
    /// </summary>
    public partial class ExamStudentWindow : Window
    {
        public ExamStudentWindow()
        {
            InitializeComponent();
        }

        private void FinishExamBTN_Click(object sender, RoutedEventArgs e)
        {
            //submit exam btn
        }

        private void PrevBtn_Click(object sender, RoutedEventArgs e)
        {
            //prev qustion btn
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            //next qustion btn
        }

        private void QuestionsLST_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // questions list box
        }
    }
}
