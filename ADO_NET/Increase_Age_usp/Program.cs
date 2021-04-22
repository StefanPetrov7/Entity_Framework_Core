using System;
using Microsoft.Data.SqlClient;

namespace Increase_Age_Stored_Procedure
{
    class Program
    {
        // Note => works only with valid input.

        const string sqlConnectionStr = "Data Source = localhost; Database={0} ;User Id = sa; Password=Password123@jkl#";
        const string DATABASE = "MinionsDB";

        static void Main(string[] args)
        {
            int id = int.Parse(Console.ReadLine());

            string usp_GetOlder = @"EXEC dbo.usp_GetOlder @Id";
            string printNames = @"SELECT Name, Age FROM Minions WHERE Id = @Id";

            using (SqlConnection sqlConnection = new SqlConnection(string.Format(sqlConnectionStr, DATABASE)))
            {
                sqlConnection.Open();
                GetOlder(sqlConnection, usp_GetOlder, id);
                PrintNames(sqlConnection, printNames, id);
            }
        }

        private static void PrintNames(SqlConnection sqlConnection, string printNames, int id)
        {
            using (SqlCommand cmd = new SqlCommand(printNames, sqlConnection))
            {
                cmd.Parameters.Add(new SqlParameter("@Id", id));

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    Console.WriteLine($"{reader[0]} – {reader[1]} years old");
                }
            }
        }

        private static void GetOlder(SqlConnection sqlConnection, string usp_GetOlder, int id)
        {
            using (SqlCommand cmd = new SqlCommand(usp_GetOlder, sqlConnection))
            {
                cmd.Parameters.Add(new SqlParameter("@Id", id));
                cmd.ExecuteNonQuery();
            }
        }
    }
}
