using Microsoft.EntityFrameworkCore;
using QuizAppAPI.Model.Entity;

namespace QuizAppAPI.Data
{
    public class QuizDbContext : DbContext
    {
        public QuizDbContext(DbContextOptions<QuizDbContext> options) : base (options) 
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<QandA> QandAs { get; set; }
    }
}
