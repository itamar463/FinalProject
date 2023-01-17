using FinalProject.Server.API.Models;

namespace FinalProject.Server.API.Context
{
    public static class InsertUsers
    {
        public static void Seed(this UsersContext dbContext)
        {
            if (!dbContext.Users.Any())
            {
                for(int i=1; i < 6; ++i)
                {
                    dbContext.Add(new Student
                    {
                        Age = 30 + i,
                        Faculty = "cs",
                        Name = "S" + i.ToString(),
                        Password = "123" + i.ToString(),
                        IsTeacher = false
                    });
                    dbContext.Add(new Teacher
                    {
                        Age = 30 + i,
                        Faculty = "cs",
                        Name = "T" + i.ToString(),
                        Password = "1234" + i.ToString(),
                        IsTeacher = true
                    });


                }
                dbContext.SaveChanges();
            }
        }
    }
}
