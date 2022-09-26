using ConsoleTableExt;
using Microsoft.VisualBasic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.PortableExecutable;

namespace Flashcards
{
        public class ConnectionBuilder
        {
            public string ConnectionString { get; set; }

        public ConnectionBuilder(string connString)
        {
            ConnectionString = connString;
        }

            public void SQLCommunicator(string command)
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    using (var tableCmd = connection.CreateCommand())
                    {
                        connection.Open();
                        {
                            tableCmd.CommandText = command;
                            tableCmd.ExecuteNonQuery();
                        }
                    }
                }
            }

        public (List<Stack> a,List<Flashcard> b, List<StudySession> c) GetTableData(string table,bool flashcardPrint,int amount, bool print)
        {
            Console.Clear();
            List<Stack> stackData = new();
            List<Flashcard> flashcardData = new();
            List<StudySession> sessionData = new();
            using (var connection = new SqlConnection(ConnectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    if (!flashcardPrint)
                        tableCmd.CommandText = $"SELECT *  FROM {table}";
                    else tableCmd.CommandText = $"SELECT *  FROM {table} WHERE StackID = {amount}";
                    SqlDataReader reader = tableCmd.ExecuteReader();

                    switch(table)
                    {
                        default: return (stackData, flashcardData, sessionData);
                        case "Stack":
                            stackData = GetStack(reader);
                            if (print)
                            {
                                List<ListStack> stackList = new();
                                for (int i = 0; i < stackData.Count; i++)
                                    stackList.Add(new ListStack(stackData[i]));
                                ConsoleTableBuilder.From(stackList).ExportAndWriteLine(TableAligntment.Center);
                            }
                            return (stackData, flashcardData, sessionData);                           
                        case "Flashcard":
                            flashcardData = GetFlashcard(reader);
                            if(print)
                            {
                                List<ListFlashcard> flashcardList = new();
                                for (int i = 0; i < flashcardData.Count; i++)
                                    flashcardList.Add(new ListFlashcard(i + 1, flashcardData[i]));
                                ConsoleTableBuilder.From(flashcardList).ExportAndWriteLine(TableAligntment.Center);
                            }
                            return (stackData, flashcardData, sessionData);
                        case "StudySessions":
                            sessionData = GetSession(reader);
                            if(print)
                            {
                                ConsoleTableBuilder.From(sessionData).ExportAndWriteLine(TableAligntment.Center);
                            }
                            return (stackData, flashcardData, sessionData);
                    }
                }
            }
        }

        public List<Stack> GetStack(SqlDataReader reader)
        {
            var stackData = new List<Stack>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    stackData.Add(new Stack(reader.GetInt32(0),reader.GetString(1)));     
                }
            }
            return stackData;
        }

        public List<Flashcard> GetFlashcard(SqlDataReader reader)
        {
            var flashcardData = new List<Flashcard>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    flashcardData.Add(new Flashcard(reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetString(2)));
                }
            }
            return flashcardData;
        }

        public List<StudySession> GetSession(SqlDataReader reader)
        {
            var sessionData = new List<StudySession>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    sessionData.Add(new StudySession(reader.GetInt32(0),
                            Convert.ToDateTime(reader.GetDateTime(2)),
                            (reader.GetInt32(3))));
                }
            }
            return sessionData;
        }

        public void Insert(string table,int ID,int score)
        {
            switch (table)
            {
                case "Stack":
                    Console.WriteLine("What would you like to name your new stack?");
                    string newStack = DataValidation.EmptyStringCheck();
                    SQLCommunicator($"INSERT INTO Stack(Name) VALUES('{newStack}')");
                    break;
                case "Flashcard":
                    Console.WriteLine($"What will the Front Side of the Flashcard say? ");
                    string frontText = DataValidation.EmptyStringCheck();
                    Console.WriteLine($"What will the Back Side of the Flashcard say? ");
                    string backText = DataValidation.EmptyStringCheck();
                    SQLCommunicator($"INSERT INTO Flashcard(FrontText,BackText,StackID) VALUES('{frontText}','{backText}','{ID}')");
                    break;
                case "StudySessions":
                    SQLCommunicator($"INSERT INTO StudySessions(StackID,Date,Score) VALUES('{ID}','{DateTime.Now}','{score}')");
                    break;

            }
            if (table != "StudySessions")
                Console.WriteLine("The entry has been added.");
            else Console.WriteLine("Your study result has been saved.");
            Program.Pause();
        }

        public void Delete(string table, int ID)
            {
                
                switch (table)
                {
                    case "Stack":
                        SQLCommunicator($"ALTER TABLE Flashcard NOCHECK CONSTRAINT ALL;" +
                            $"ALTER TABLE StudySessions NOCHECK CONSTRAINT ALL;" +
                            $"DELETE FROM Stack WHERE StackID='{ID}';" +
                            $"DELETE FROM Flashcard WHERE StackID='{ID}';" +
                            $"DELETE FROM StudySessions WHERE StackID='{ID}';" +
                            $"ALTER TABLE StudySessions CHECK CONSTRAINT ALL;" +
                            $"ALTER TABLE Flashcard CHECK CONSTRAINT ALL;");
                        break;
                    case "Flashcard":
                    SQLCommunicator($"DELETE FROM Flashcard WHERE FlashcardID='{ID}'");
                        break;
                }
            Console.WriteLine("The entry has been deleted.");
                Program.Pause();
            }

            public void Update(string table,int ID)
            {
                switch (table)
            {
                case "Stack":
                    Console.WriteLine($"What would you like to rename it?");                  
                    string newStack = DataValidation.EmptyStringCheck();
                    SQLCommunicator($"UPDATE Stack SET Name='{newStack}' WHERE StackID='{ID}'");
                    break;
                case "Flashcard":
                    Console.WriteLine($"What will the Front Side of the Flashcard say? ");
                    string frontText = DataValidation.EmptyStringCheck();
                    Console.WriteLine($"What will the Back Side of the Flashcard say? ");
                    string backText = DataValidation.EmptyStringCheck();
                    SQLCommunicator($"UPDATE Flashcard SET FrontText='{frontText}',BackText='{backText}' WHERE FlashcardID = '{ID}'");
                    break;
            }
                Console.WriteLine("The entry has been updated.");
                Program.Pause();
            }
        }
}
