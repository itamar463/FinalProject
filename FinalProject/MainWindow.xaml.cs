using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FinalProject.Demos.Objects;

namespace FinalProject.Demos
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //IPersonsReposetory studentsRepository;
        //IPersonsReposetory teachersRepository;
        public MainWindow()
        {
            InitializeComponent();
            
        }

        

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            string? name = textBoxName.Text;
            string? id = textBoxId.Text;

            if (name != null && id != null)
            {
                StudentsRepository studentRepo = StudentsRepository.Instance;
                TeachersRepository teacherRepo = TeachersRepository.Instance;
                Student? student = studentRepo.Students.Where(s => s.Id == id).SingleOrDefault();
                Teacher? teacher = teacherRepo.Teachers.Where(t => t.Id == id).SingleOrDefault();

                //now its only for check need to identify by name and password
                if (student != null)
                {
                    StudentWindow s = new StudentWindow(studentRepo, student);
                    s.Show();
                }
                else if(teacher != null)
                {
                    TeacherWindow t = new TeacherWindow(teacherRepo,teacher);
                    t.Show();
                }
                else
                {
                    MessageBox.Show("Didn't find user!!!");
                }
            }

        }


    }
}
