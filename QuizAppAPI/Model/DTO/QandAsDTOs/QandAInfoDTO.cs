using QuizAppAPI.Model.Entity;

namespace QuizAppAPI.Model.DTO.QandAs
{
    public class QandAInfoDTO
    {
        public int QuizId { get; set; }
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }

    }
}
