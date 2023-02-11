using System.Linq;
using System.Windows;
using FinalProject.Demos.Objects;

namespace FinalProject.Demos
{

    public partial class MainWindow : Window
    {
        // the main window of the app is a login page -
        // it will receive a person's name and password and will enter the student/teacher next window accordingly to the persons location on the DB (Student/Teacher window)
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            //verify person exist by his name and password
            string? name = textBoxName.Text;
            string? password = textBoxPass.Password;

            if (name != "" && password != "")
            {
                StudentsRepository studentRepo = StudentsRepository.Instance;
                TeachersRepository teacherRepo = TeachersRepository.Instance;
                Student student = studentRepo.Students.Where(s => (s.Password == password && s.Name == name)).SingleOrDefault();
                Teacher teacher = teacherRepo.Teachers.Where(t => (t.Password == password && t.Name == name)).SingleOrDefault();

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
                    MessageBox.Show("Name or Password are incorrect.");
                }
            }
            else
            {
                MessageBox.Show("One or Two of the fields is missing.");
            }

        }


    }
}
