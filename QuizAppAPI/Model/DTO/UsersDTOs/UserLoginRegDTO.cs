namespace QuizAppAPI.Model.DTO.Users
{
    public class UserLoginRegDTO
    {
        public int UserId { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }

    }
}
