using Microsoft.EntityFrameworkCore;
using FinalProject.Server.API.Models;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata;

namespace FinalProject.Server.API.Context
{
    
    public class UsersContext: DbContext
    {
        public UsersContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<Person> Users { get; set; } = null!;
        public DbSet<Exam> Exams { get; set; } = null!;
        public DbSet<Question> Questions { get; set; } = null!;

        
    }
    
    }
