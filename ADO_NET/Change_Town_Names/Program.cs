using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace Change_Town_Names_Casing
{
    class Program
    {
        const string sqlConnectionStr = "Data Source = localhost; Database={0} ;User Id = sa; Password=Password123@jkl#";

        static void Main(string[] args)
        {
            string DATABASE = "MinionsDB";


            string queryTurnToUpper = @"UPDATE Towns
                                             SET Name = UPPER(Name)
                                             WHERE CountryCode = (SELECT Id FROM Countries WHERE Name = @Country)";

            string getTowns = @"SELECT t.Name
                                            FROM Towns AS t
                                            LEFT JOIN Countries AS c ON c.Id = t.CountryCode 
                                            WHERE c.Name = @Country";

            string country = Console.ReadLine();

   

            using (SqlConnection sqlConnection = new SqlConnection(string.Format(sqlConnectionStr, DATABASE)))
            {
                sqlConnection.Open();

                int townsCount = TurnToUpper(queryTurnToUpper, sqlConnection, country);

                if (townsCount == 0)
                {
                    Console.WriteLine("No town names were affected.");
                }
                else
                {
                    List<string> townNames = GetTownNames(getTowns, sqlConnection, country);
                    Console.WriteLine($"{townsCount} town names were affected. ");
                    Console.WriteLine($"[{string.Join(", ", townNames)}]");
                }
            }
        }

        private static List<string> GetTownNames(string getTownNames, SqlConnection sqlConnection, string country)
        {
            List<string> towns = new List<string>();

            using (SqlCommand getTownNamesCmd = new SqlCommand(getTownNames, sqlConnection))
            {
                getTownNamesCmd.Parameters.Add(new SqlParameter("@Country", country));

                SqlDataReader reader = getTownNamesCmd.ExecuteReader();

                while (reader.Read())
                {
                    string town = reader[0].ToString();
                    towns.Add(town);
                }
            }
            return towns;
        }

        private static int TurnToUpper(string queryGetAffectedRows, SqlConnection sqlConnection, string country)
        {
            using (SqlCommand toUpperCmd = new SqlCommand(queryGetAffectedRows, sqlConnection))
            {
                toUpperCmd.Parameters.Add(new SqlParameter("@Country", country));

                int? townsCount = (int)toUpperCmd.ExecuteNonQuery();

                return (int)townsCount;
            }
        }
    }
}
