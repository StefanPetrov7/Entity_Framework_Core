using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace Print_All_Minion_Names
{
    class Program
    {
        const string sqlConnectionStr = "Data Source = localhost; Database={0} ;User Id = sa; Password=Password123@jkl#";
        const string DATABASE = "MinionsDB";

        static void Main(string[] args)
        {
            string getNamesQuery = @"SELECT Name FROM Minions";

            using (SqlConnection sqlConnection = new SqlConnection(string.Format(sqlConnectionStr, DATABASE)))
            {
                sqlConnection.Open();

                List<string> minions = GetMinions(sqlConnection, getNamesQuery);
                List<string> arranged = new List<string>();
                int count = minions.Count - 1;

                for (int i = 0; i < minions.Count / 2; i++)
                {
                    arranged.Add(minions[i]);
                    arranged.Add(minions[count - i]);
                }

                if (minions.Count % 2 != 0)
                {
                    arranged.Add(minions[minions.Count / 2]);
                }

                Console.WriteLine(string.Join(", ", arranged));
            }
        }

        private static List<string> GetMinions(SqlConnection sqlConnection, string getNamesQuery)
        {
            List<string> minions = new List<string>();

            using (SqlCommand cmd = new SqlCommand(getNamesQuery, sqlConnection))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        minions.Add(reader[0]?.ToString());
                    }
                }
            }
            return minions;
        }
    }
}
