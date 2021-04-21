using System;
using Microsoft.Data.SqlClient;

namespace Minion_Names
{
    class Program
    {
        const string connectionString = "Data Source=localhost;Database={0} ;User Id=sa;Password=Password123@jkl#";

        static void Main(string[] args)
        {
            int idVillain = int.Parse(Console.ReadLine());

            string queryMinionsNames = @"SELECT ROW_NUMBER() OVER (ORDER BY m.Name) AS [RowNum],
                                                m.Name,
                                                m.Age
                                            FROM MinionsVillains AS mv
                                            JOIN Minions AS m ON m.Id = mv.MinionId
                                            WHERE mv.VillainId = @Parameter";

            string queryVillainName = @"SELECT [Name]
                                            FROM Villains
                                            WHERE Id = @Parameter";

            using (SqlConnection sqlConnection = new SqlConnection(string.Format(connectionString, "MinionsDB")))
            {
                sqlConnection.Open();

                using (SqlCommand command = new SqlCommand(queryVillainName, sqlConnection))
                {
                    command.Parameters.Add(new SqlParameter("@Parameter", idVillain));

                    var villainName = command.ExecuteScalar();

                    if (villainName != null)
                    {
                        Console.WriteLine($"Villain: {villainName}");
                    }
                    else
                    {
                        Console.WriteLine($"No villain with ID {idVillain} exists in the database.");
                        Environment.Exit(0);
                    }
                }

                using (SqlCommand command = new SqlCommand(queryMinionsNames, sqlConnection))
                {

                    try
                    {
                        command.Parameters.Add(new SqlParameter("@Parameter", idVillain));

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                Console.WriteLine("(no minions)");
                            }
                            else
                            {
                                while (reader.Read())
                                {
                                    string rowNum = reader[0]?.ToString();
                                    string name = reader[1]?.ToString();
                                    string age = reader[2]?.ToString();
                                    Console.WriteLine($"{rowNum}. {name} {age}");
                                }
                            }
                            
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }
    }
}
