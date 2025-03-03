namespace QuizAppAPI.Model.Entity
{
    public class User
    {
        public int UserId { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required string Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }


        public ICollection<Quiz>? Quizzes { get; set; }

    }
}
