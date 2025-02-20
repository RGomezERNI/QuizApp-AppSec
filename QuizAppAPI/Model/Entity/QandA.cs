namespace QuizAppAPI.Model.Entity
{
    public class QandA
    {
        public int Id { get; set; }
        public int QuizId {  get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }

        public Quiz Quizzes { get; set; }

    }
}
