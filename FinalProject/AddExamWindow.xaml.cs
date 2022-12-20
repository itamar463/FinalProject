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
    /// Interaction logic for AddExamWindow.xaml
    /// </summary>
    public partial class AddExamWindow : Window
    {
        public AddExamWindow()
        {
            InitializeComponent();
        }

        private void RemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            // remove qeustion btn
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            // add wuestion btn
        }

        private void QuestionsLST_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // questions listbox
        }

        private void RandomizeQuestions_Click(object sender, RoutedEventArgs e)
        {
            // rand the questions btn
        }

        private void UploadExamBTN_Click(object sender, RoutedEventArgs e)
        {
            // upload exam to db btn
        }

        private void SaveExamBTN_Click(object sender, RoutedEventArgs e)
        {
            // save exam localy btn
        }

        private void IsCorrectAnswer1_Checked(object sender, RoutedEventArgs e)
        {
            // if answer 1 is the correct answer checkbox
        }

        private void IsCorrectAnswer2_Checked(object sender, RoutedEventArgs e)
        {
            // if answer 2 is the correct answer checkbox
        }

        private void IsCorrectAnswer3_Checked(object sender, RoutedEventArgs e)
        {
            // if answer 3 is the correct answer checkbox
        }

        private void IsCorrectAnswer4_Checked(object sender, RoutedEventArgs e)
        {
            // if answer 4 is the correct answer checkbox
        }

        private void IsImageQuestion_Checked(object sender, RoutedEventArgs e)
        {
            // if question is image btn
        }
    }
}
