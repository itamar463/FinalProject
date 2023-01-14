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
    /// Interaction logic for StudentWindow.xaml
    /// </summary>
    public partial class StudentWindow : Window
    {
        StudentsRepository repo;

        Student stud;
        public StudentWindow()
        {
            InitializeComponent();
        }
        public StudentWindow(StudentsRepository repo, Student s)
        {
            InitializeComponent();
            this.repo = repo;
            this.stud = s;
            StudentLbl.Content += stud.Name;
        }

        private void EnterExam_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
