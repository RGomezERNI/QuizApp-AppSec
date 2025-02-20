using QuizAppAPI.Model.DTO.Quiz;

namespace QuizAppAPI.Model.DTO.Users
{
    public class UserInfoDTO
    {
        public int UserId { get; set; }
        public required string UserName { get; set; }

        public ICollection<QuizInfoDTO> QuizInfoDTOs { get; set; }
    }
}
