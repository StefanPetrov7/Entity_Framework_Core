using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;

namespace Increase_Minion_Age
{
    class Program
    {
        const string sqlConnectionStr = "Data Source = localhost; Database={0} ;User Id = sa; Password=Password123@jkl#";
        const string DATABASE = "MinionsDB";

        static void Main(string[] args)
        {
            int[] ids = Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

            string querySetAgeAndTitle = @"UPDATE Minions
                                            SET Name = CONCAT(UPPER(LEFT(Name,1)), RIGHT(Name,LEN(Name)-1)), Age+=1
                                            WHERE Id = @Id";    

            string queryGetMinion = @"SELECT Name, Age FROM Minions WHERE Id = @Id";

            using (SqlConnection sqlConnection = new SqlConnection(string.Format(sqlConnectionStr, DATABASE)))
            {
                sqlConnection.Open();
                Dictionary<string, string> minions = new Dictionary<string, string>();

                for (int i = 0; i < ids.Length; i++)
                {
                    IncreaseAgeAndTitleCase(sqlConnection, ids[i], querySetAgeAndTitle);
                    string[] data = GetMinion(sqlConnection, ids[i], queryGetMinion);
                    minions.Add(data[0], data[1]);
                }

                foreach (var minion in minions)
                {
                    Console.WriteLine($"{minion.Key}, {minion.Value}");
                }
            }
        }

        private static string[] GetMinion(SqlConnection sqlConnection, int id, string queryGetMinion)
        {
            string[] data = new string[2];

            using (SqlCommand cmd = new SqlCommand(queryGetMinion, sqlConnection))
            {
                cmd.Parameters.Add(new SqlParameter("Id", id));
                SqlDataReader reader = cmd.ExecuteReader();
                reader.Read();
                data[0] = reader[0]?.ToString();
                data[1] = reader[1]?.ToString();
                reader.Close();
            }

            return data;
        }

        private static void IncreaseAgeAndTitleCase(SqlConnection sqlConnection, int id, string queryAgeIncrease)
        {
            using (SqlCommand cmd = new SqlCommand(queryAgeIncrease, sqlConnection))
            {
                cmd.Parameters.Add(new SqlParameter("Id", id));
                cmd.ExecuteNonQuery();
            }
        }
    }
}