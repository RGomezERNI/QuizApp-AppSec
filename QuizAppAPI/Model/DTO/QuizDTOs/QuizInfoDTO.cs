using QuizAppAPI.Model.DTO.QandAs;
using QuizAppAPI.Model.DTO.Users;
using QuizAppAPI.Model.Entity;

namespace QuizAppAPI.Model.DTO.Quiz
{
    public class QuizInfoDTO
    {
        public int UserId { get; set; }
        public int quizId { get; set; }
        public string QuizName { get; set; }
        public string QuizTopic { get; set; }

        public ICollection<QandAInfoDTO> QandAs { get; set; }
    }
}
