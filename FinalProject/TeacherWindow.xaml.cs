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
    /// Interaction logic for TeacherWindow.xaml
    /// </summary>
    public partial class TeacherWindow : Window
    {
        private TeachersRepository repo;

        private Teacher teach;
        public TeacherWindow()
        {
            InitializeComponent();
        }

        public TeacherWindow(TeachersRepository repo, Teacher t)
        {
            InitializeComponent();
            this.repo = repo;
            this.teach = t;
            Teacherlbl.Content += t.Name;
        }


        private void UpdateExamBTN_Click(object sender, RoutedEventArgs e)
        {
            //update exam button
        }

        private void AddExamBTN_Click(object sender, RoutedEventArgs e)
        {
            //add exam button
        }

        private void RemoveExamBTN_Click(object sender, RoutedEventArgs e)
        {
            //remove exam button
        }

        private void ExamsCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //combo bex for teacher to choose one of her exams
        }
    }
}
