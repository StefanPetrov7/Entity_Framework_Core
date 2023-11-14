using System;
using Microsoft.Data.SqlClient;

namespace Add_Minion
{
    class Program
    {
        const string sqlConnectionStr = "Data Source = localhost; Database={0} ;User Id = sa; Password=Password123@jkl#";

        static void Main(string[] args)
        {
            string[] minionInfo = Console.ReadLine().Split();
            string[] villainInfo = Console.ReadLine().Split();
            string villainName = villainInfo[1];
            string minionName = minionInfo[1];
            int minionAge = int.Parse(minionInfo[2]);
            string townName = minionInfo[3];
            string DATABASE = "MinionsDB";

            using (SqlConnection sqlConnection = new SqlConnection(string.Format(sqlConnectionStr, DATABASE)))
            {
                sqlConnection.Open();

                string queryAddingTown = "EXEC dbo.usp_AddTownToDB @Town";

                using (SqlCommand addTownCommand = new SqlCommand(queryAddingTown, sqlConnection))
                {
                    addTownCommand.Parameters.Add(new SqlParameter("Town", townName));

                    try
                    {
                        addTownCommand.ExecuteNonQuery();
                        Console.WriteLine($"Town {townName} was added to the database.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                string addingVillain = "EXEC dbo.usp_AddVillain @VillainName";

                using (SqlCommand addVillainComamnd = new SqlCommand(addingVillain, sqlConnection))
                {

                    addVillainComamnd.Parameters.Add(new SqlParameter("@VillainName", villainName));

                    try
                    {
                        addVillainComamnd.ExecuteNonQuery();
                        Console.WriteLine($"Villain {villainName} was added to the database.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                string addMinion = "EXEC dbo.usp_AddMinion @Mname, @Age, @Town";

                using (SqlCommand addMinionCommand = new SqlCommand(addMinion, sqlConnection))
                {
                    addMinionCommand.Parameters.Add(new SqlParameter("@Mname", minionName));
                    addMinionCommand.Parameters.Add(new SqlParameter("@Age", minionAge));
                    addMinionCommand.Parameters.Add(new SqlParameter("@Town", townName));

                    try
                    {
                        addMinionCommand.ExecuteNonQuery();
                        Console.WriteLine("Successfully added minion into the Database");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                string makeServant = "EXEC dbo.usp_MakeMinionServant @Mname, @Vname";

                using (SqlCommand makeServantCommand = new SqlCommand(makeServant, sqlConnection))
                {
                    makeServantCommand.Parameters.Add(new SqlParameter("@Mname", minionName));
                    makeServantCommand.Parameters.Add(new SqlParameter("@Vname", villainName));

                    try
                    {
                        makeServantCommand.ExecuteNonQuery();
                        Console.WriteLine($"Successfully added {minionName} to be minion of {villainName}.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                };
            }
        }
    }
}
