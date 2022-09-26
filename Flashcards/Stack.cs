using System.Data.SqlClient;

namespace Flashcards
{
    public class Stack
    {
        public int Id  { get; set; }
        public string Name { get; set; }

        //constructors
        public Stack(int Id, string Name)
        {
            this.Id = Id;
            this.Name = Name;
        }
    }
    public class ListStack
    {
        public string Name { get; set; }

        //constructors
        public ListStack(Stack s)
        {
            this.Name = s.Name;
        }
    }
}