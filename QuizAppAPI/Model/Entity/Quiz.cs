namespace QuizAppAPI.Model.Entity
{
    public class Quiz
    {
        public int QuizId { get; set; }
        public int UserId {  get; set; }
        public string QuizName { get; set; }
        public string QuizTopic { get; set; }


        public User Users { get; set; }
        public ICollection<QandA>? QandAs { get; set; }
    }
}
