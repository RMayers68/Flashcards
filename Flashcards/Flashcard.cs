namespace Flashcards
{
    public class Flashcard
    {
        public int Id { get; set; }
        public string FrontText { get; set; }
        public string BackText { get; set; }

        //constructors
        public Flashcard(int Id, string FrontText, string BackText)
        {
            this.Id = Id;
            this.FrontText = FrontText;
            this.BackText = BackText;
        }
    }

    public class ListFlashcard
    {
        public int Id { get; set; }
        public string FrontText { get; set; }
        public string BackText { get; set; }

        //constructors
        public ListFlashcard(int Id, Flashcard f)
        {
            this.Id = Id;
            this.FrontText = f.FrontText;
            this.BackText = f.BackText;
        }
    }   
}
