using QuizAppAPI.Model.DTO.Users;
using QuizAppAPI.Model.Entity;

namespace QuizAppAPI.Model.DTO.Quiz
{
    public class AddUpQuizDTO
    {
        public int UserId { get; set; }
        public string QuizName { get; set; }
        public string QuizTopic { get; set; }
    }
}
