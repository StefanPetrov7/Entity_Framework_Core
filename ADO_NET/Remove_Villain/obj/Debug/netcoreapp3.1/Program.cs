using System;
using Microsoft.Data.SqlClient;

namespace Remove_Villain
{
    class Program
    {
        // Note:
        // => SQL queries can be seen in the sql. file in the same folder.
        // => Stored Procedures, etc. 

        const string sqlConnectionStr = "Data Source = localhost; Database={0} ;User Id = sa; Password=Password123@jkl#";
        const string DATABASE = "MinionsDB";

        static void Main(string[] args)
        {
            int villainId = int.Parse(Console.ReadLine());
        
            string minionCountQuery = @"SELECT COUNT(*) FROM MinionsVillains WHERE VillainId = @Id";
            string evilName = @"SELECT Name FROM Villains WHERE Id = @Id";
            string deleteEvilQuery = @"EXEC dbo.usp_DeleteVillain @Id";

            using (SqlConnection sqlConnection = new SqlConnection(string.Format(sqlConnectionStr, DATABASE)))
            {
                sqlConnection.Open();

                string villainName = GetName(sqlConnection, evilName, villainId);

                if (villainName != null)
                {
                    int minions = GetMinionCount(sqlConnection, minionCountQuery, villainId);
                    DeleteMinion(sqlConnection, deleteEvilQuery, villainId);
                    Console.WriteLine($"{villainName} was deleted.");
                    Console.WriteLine($"{minions} minions were released.");
                }
                else
                {
                    Console.WriteLine("No such villain was found.");
                }
            }
        }

        private static void DeleteMinion(SqlConnection sqlConnection, string deleteEvilQuery, int villainId)
        {
            using (SqlCommand deleteCmd = new SqlCommand(deleteEvilQuery, sqlConnection))
            {
                deleteCmd.Parameters.Add(new SqlParameter("@Id", villainId));
                deleteCmd.ExecuteNonQuery();
            }
        }

        private static string GetName(SqlConnection sqlConnection, string evilNameQuery, int villainId)
        {
            string name;

            using (SqlCommand getNameCmd = new SqlCommand(evilNameQuery, sqlConnection))
            {
                getNameCmd.Parameters.Add(new SqlParameter("@Id", villainId));
                name = getNameCmd.ExecuteScalar()?.ToString();
            }

            return name;
        }

        private static int GetMinionCount(SqlConnection sqlConnection, string minionCountQuery, int villainId)
        {
            int result;

            using (SqlCommand getCountCmd = new SqlCommand(minionCountQuery, sqlConnection))
            {
                getCountCmd.Parameters.Add(new SqlParameter("@Id", villainId));
                result = (int)getCountCmd.ExecuteScalar();
            }

            return result;
        }
    }
}
