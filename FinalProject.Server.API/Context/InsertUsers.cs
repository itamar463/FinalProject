using FinalProject.Server.API.Models;
using RandomNameGeneratorNG;

namespace FinalProject.Server.API.Context
{
    //This class creates and enters person-type (both teacher and student) users to the DB
    public static class InsertUsers
    {
        public static void Seed(this UsersContext dbContext)
        {
            
            if (!dbContext.Users.Any())
            {
                Random rand = new Random();
               //TODO: is this relevant?
               //List<string> Fac = new List<string> { "Computer Science", "Philosophy","Economy","Social Work" };
                var PersonGenerator = new PersonNameGenerator();
                for(int i=1; i < 5; ++i)
                {
                    dbContext.Add(new Student
                    {
                        Age = rand.Next(20, 40),
                        Name = PersonGenerator.GenerateRandomFirstName() + " " + PersonGenerator.GenerateRandomLastName(),
                        Password = "123" + i.ToString(),
                        IsTeacher = false
                    });
                    dbContext.Add(new Teacher
                    {

                        Age = rand.Next(40, 67),
                        Name = PersonGenerator.GenerateRandomFirstName() + " " + PersonGenerator.GenerateRandomLastName(),
                        Password = "1234" + i.ToString(),
                        IsTeacher = true
                    });
                    dbContext.Add(new Student
                    {
                        Age = rand.Next(20, 40),
                        Name = PersonGenerator.GenerateRandomFirstName() + " " + PersonGenerator.GenerateRandomLastName(),
                        Password = "234" + i.ToString(),
                        IsTeacher = false
                    });
                    dbContext.Add(new Student
                    {
                        Age = rand.Next(20, 40),
                        Name = PersonGenerator.GenerateRandomFirstName() + " " + PersonGenerator.GenerateRandomLastName(),
                        Password = "345" + i.ToString(),
                        IsTeacher = false
                    });
                    dbContext.Add(new Student
                    {
                        Age = rand.Next(20, 40),
                        Name = PersonGenerator.GenerateRandomFirstName() + " " + PersonGenerator.GenerateRandomLastName(),
                        Password = "456" + i.ToString(),
                        IsTeacher = false
                    });
                }
                dbContext.SaveChanges();
            }
        }
    }
}
