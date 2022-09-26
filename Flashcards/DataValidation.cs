using System.Data.SqlClient;
using System.Globalization;

namespace Flashcards
{
    public class DataValidation
    {
        public static CultureInfo enUS = new CultureInfo("en-US");

        public static DateTime InputTime(string StartOrEnd)
        {
            DateTime dT;
            Console.Clear();
            Console.WriteLine($"Please enter the {StartOrEnd} time in this format (and this format only)\nMM/dd/YYYY hh:mm AM/PM");
            while (!DateTime.TryParseExact(Console.ReadLine(), "g", enUS, DateTimeStyles.AllowLeadingWhite, out dT))
                Console.WriteLine("Incorrect format, please enter time in this format: MM-dd-YYYY hh:mm");
            return dT;
        }

        public static string EmptyStringCheck()
        {
            string a = Console.ReadLine();
            while (String.IsNullOrWhiteSpace(a))
            {
                Console.WriteLine("Invalid input, enter again:");
                a = Console.ReadLine();
            }
            return a;
        }

        public static int ListCountCheck(int count)
        {
            int a;
            while (!Int32.TryParse(Console.ReadLine(), out a) || a < 1 || a > count)
                Console.WriteLine("Invalid input, enter again:");
            return a;
        }

        public static int MenuInputCheck(int count)
        {
            int a;
            while (!Int32.TryParse(Console.ReadLine(), out a) || a < 0 || a > count)
                Console.WriteLine("Invalid input, enter again:");
            return a;
        }
        public static (bool validName,int ID) CheckStackName(string table,string name, ConnectionBuilder b)
        {
            Console.Clear();
            if (name == "0")
                return (true, 0);
            using (var connection = new SqlConnection(b.ConnectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText =
                        $"SELECT * FROM {table} WHERE Name='{name}'";

                    SqlDataReader reader = tableCmd.ExecuteReader();

                    if (!reader.HasRows)
                    {
                        Console.WriteLine("Invalid input, enter again:");
                        return (false,0);
                    }                       
                    while(reader.Read()) return (true,reader.GetInt32(0));
                }
            }
            return (false, 0);
        }
    }
}

