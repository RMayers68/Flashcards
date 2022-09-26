using System.Configuration;
using System.Collections.Specialized;
using System.Web;

namespace Flashcards
{
    class Program
    {
        static void Main(string[] args)
        {
            string connString = GetConnectionStringByName("Name"), stack = "Stack", flashcard = "Flashcard", session = "StudySessions";
            ConnectionBuilder builder = new ConnectionBuilder(connString);
            int mainMenu = 4;
            while (mainMenu != 0)                                                     //MAIN MENU
            {
                string stackSelection = "";
                Console.Clear();
                Console.WriteLine($"{Tab(6)}Flashcard Study App\n\n{MenuLine()}\n\n{Tab(5)}0: Exit Program\n\n{Tab(5)}1: Manage Stacks\n\n{Tab(5)}2: Manage Flashcards\n\n" +
                    $"{Tab(5)}3: Study\n\n{Tab(5)}4: View your Study Sessions\n\n{MenuLine()}");
                mainMenu = DataValidation.MenuInputCheck(4);
                switch (mainMenu)
                {
                    default:
                        Console.WriteLine("Goodbye!");
                        Console.ReadKey();
                        break;
                    case 1:                                                             // Manage Stacks
                        int stackMenu = 3;                        
                        while (stackMenu != 0)
                        {
                            Console.Clear();
                            builder.GetTableData(stack, false, 0, true);
                            int stackID=0;
                            Console.WriteLine($"{Tab(6)}Manage Stacks\n\n{MenuLine()}\n\n{Tab(5)}0: Go Back\n\n{Tab(5)}1: Create new Stack\n\n{Tab(5)}2: Update Stack Name\n\n" +
                                $"{Tab(5)}3: Delete Stack\n\n{MenuLine()}");
                            stackMenu = DataValidation.MenuInputCheck(3);
                            switch (stackMenu)
                            {
                                default:
                                    break;
                                case 1:                                                 // Create Stack
                                    builder.Insert(stack,0,0);
                                    break;
                                case 2:                                                 // Update Stack Name
                                    Console.WriteLine("Please enter the entry you would like to update: ");
                                    stackSelection = DataValidation.EmptyStringCheck();
                                    while (!DataValidation.CheckStackName(stack, stackSelection, builder).validName)
                                    {
                                        stackSelection = DataValidation.EmptyStringCheck();
                                    }
                                    stackID = DataValidation.CheckStackName(stack, stackSelection, builder).ID;
                                    builder.Update(stack, stackID);
                                    break;
                                case 3:                                                 // Delete Stack
                                    Console.WriteLine("Please enter the entry you would like to delete: ");
                                    stackSelection = DataValidation.EmptyStringCheck();
                                    while (!DataValidation.CheckStackName(stack, stackSelection, builder).validName)
                                    {
                                        stackSelection = DataValidation.EmptyStringCheck();
                                    }
                                    stackID = DataValidation.CheckStackName(stack, stackSelection, builder).ID;
                                    builder.Delete(stack, stackID);
                                    break;
                            }

                        }   
                        break;
                    case 2:                                                             // Manage Flashcards
                        while (stackSelection != "0")
                        {
                            int flashcardMenu = 6;
                            Console.Clear();
                            builder.GetTableData(stack, false, 0, true);
                            Console.WriteLine($"Choose a stack of flashcards to interact with or 0 to exit:");
                            stackSelection = DataValidation.EmptyStringCheck();
                            while(!DataValidation.CheckStackName(stack,stackSelection,builder).validName)
                            {
                                stackSelection = DataValidation.EmptyStringCheck();
                            }
                            int stackID = DataValidation.CheckStackName(stack, stackSelection, builder).ID;
                            while (flashcardMenu > 1 && stackSelection != "0")
                            {
                                int flashcardID = 0;
                                List<Flashcard> flashcardList = new();
                                Console.Clear();
                                Console.WriteLine($"{MenuLine()}\n{Tab(5)}Current Working Stack: {stackSelection}\n\n{Tab(5)}0: Return to Main Menu\n{Tab(5)}1: Change Current Stack\n" +
                                $"{Tab(5)}2: View All Flashcards\n{Tab(5)}3:Create a Flashcard\n{Tab(5)}4:Edit a Flashcard\n{Tab(5)}5:Delete a Flashcard\n{MenuLine()}");
                                flashcardMenu = DataValidation.MenuInputCheck(5);
                                switch (flashcardMenu)
                                {
                                    default:                                                // Return to Main Menu
                                        stackSelection = "0";
                                        break;
                                    case 1:                                                 // Change Stack
                                        break;
                                    case 2:                                                 // View all Flashcards
                                        builder.GetTableData(flashcard,true,stackID,true);
                                        Console.ReadKey();
                                        break;
                                    case 3:                                                 // Create a Flashcard   
                                        builder.Insert(flashcard,stackID,0);
                                        break;
                                    case 4:                                                 // Edit a Flashcard
                                        flashcardList = builder.GetTableData(flashcard, true, stackID, true).b;
                                        Console.WriteLine("Please enter the entry you would like to update: ");
                                        int update = DataValidation.ListCountCheck(flashcardList.Count);
                                        flashcardID = flashcardList[update-1].Id;
                                        builder.Update(flashcard,flashcardID);
                                        break;
                                    case 5:                                                 // Delete a Flashcard
                                        flashcardList = builder.GetTableData(flashcard, true,stackID, true).b;
                                        Console.WriteLine("Please enter the entry you would like to delete: ");
                                        int delete = DataValidation.ListCountCheck(flashcardList.Count);
                                        flashcardID = flashcardList[delete-1].Id;
                                        builder.Delete(flashcard, flashcardID);
                                        break;                                          
                                }
                            }
                        }
                        break;
                    case 3:                                                             // Study
                        while (stackSelection != "0")
                        {
                            int score = 0;
                            string answer = "";
                            List<Flashcard> studyList = new();
                            Console.Clear();
                            int count = builder.GetTableData(stack, false, 0,true).a.Count;
                            Console.WriteLine($"Choose a stack of flashcards to study or 0 to exit:");
                            stackSelection = DataValidation.EmptyStringCheck();
                            while (!DataValidation.CheckStackName(stack, stackSelection, builder).validName)
                            {
                                stackSelection = DataValidation.EmptyStringCheck();
                            }
                            int stackID = DataValidation.CheckStackName(stack, stackSelection, builder).ID;
                            while (stackSelection != "0")
                            {
                                studyList = builder.GetTableData(flashcard, true, stackID,false).b;
                                foreach(var question in studyList)
                                {
                                    Console.WriteLine(question.FrontText);
                                    answer = DataValidation.EmptyStringCheck();
                                    if (answer.ToLower() == question.BackText.ToLower())
                                    {
                                        Console.WriteLine("Correct!");
                                        score++;
                                    }
                                    else Console.WriteLine("Incorrect!");
                                    Console.Clear();
                                }
                                Console.WriteLine($"You got {score}/{studyList.Count} right!");
                                builder.Insert(session,stackID, score);
                                stackSelection = "0";
                            }
                        }
                        break;
                    case 4:                                                             // View StudySessions
                        builder.GetTableData(session,false,0,true);
                        Pause();
                        break;
                }
            }
        }

        // Menu UI Methods
        public static string Tab(int i)
        {
            string tab = "";
            for (int j = 0; j < i; j++)
                tab += "\t";
            return tab;
        }

        public static string MenuLine()
        {
            string a = $"{Tab(5)}********************************";
            return a; 
        }
        public static void Pause()
        {
            Console.WriteLine("\nPress any key to continue...\n");
            Console.ReadKey();
        }

        // Retrieves a connection string by name.
        // Returns null if the name is not found.
        static string GetConnectionStringByName(string name)
        {
            // Assume failure.
            string returnValue = null;

            // Look for the name in the connectionStrings section.
            ConnectionStringSettings settings =
                ConfigurationManager.ConnectionStrings[name];

            // If found, return the connection string.
            if (settings != null)
                returnValue = settings.ConnectionString;

            return returnValue;
        }
    }
}