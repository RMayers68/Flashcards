
namespace Flashcards
{
    public class StudySession
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Score { get; set; }

        //constructors
        public StudySession(int Id, DateTime Date, int Score)
        {
            this.Id = Id;
            this.Date = Date;
            this.Score = Score;
        }


        // string method
        public override string ToString()
        {
            return $"{Date}: \nScore: {Score}\n";
        }
    }
}
