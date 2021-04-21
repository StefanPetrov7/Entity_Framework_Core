using System;
using Microsoft.Data.SqlClient;

namespace Add_Minion_Review
{
    class Program
    {
        // Note:
        // Queries are working only for unique Minions and Villains names. !!!!
        // If same minion with the same name is added twice, it will throw and exception.
        // Query is searching for a minion ID by minion Name !!!! Queries need to be adjusted. :)
        // This was not made clear by the condition of the task!!

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

            string queryGetTownId = "SELECT Id FROM Towns WHERE Name = @Town";

            string queryCreateTown = "INSERT INTO Towns (Name, CountryCode)  VALUES (@Town, 1)";

            string queryGetVillainId = "SELECT Id FROM Villains WHERE Name = @Name";

            string createVillain = "INSERT INTO Villains (Name, EvilnessFactorId) VALUES (@Name, 4)";

            string createMinion = "INSERT INTO Minions (Name, Age, TownId) VALUES (@Name, @Age, @TownID)";

            string getMinionId = "SELECT Id FROM Minions WHERE Name = @Name";

            string insertIntoMappingTable = "INSERT INTO MinionsVillains (MinionId,VillainId) VALUES (@Mid, @Vid)";

            using (SqlConnection sqlConnection = new SqlConnection(string.Format(sqlConnectionStr, DATABASE)))
            {
                sqlConnection.Open();
                int townId;
                int villainId;
                int minionId;

                using (SqlCommand getTownIdCommand = new SqlCommand(queryGetTownId, sqlConnection))
                {
                    getTownIdCommand.Parameters.Add(new SqlParameter("@Town", townName));

                    bool isParsed = int.TryParse(getTownIdCommand.ExecuteScalar()?.ToString(), out townId);

                    if (!isParsed)
                    {
                        using (SqlCommand createTownCommand = new SqlCommand(queryCreateTown, sqlConnection))
                        {
                            createTownCommand.Parameters.AddWithValue("@Town", townName);

                            createTownCommand.ExecuteNonQuery();

                            townId = (int)getTownIdCommand.ExecuteScalar();

                            Console.WriteLine($"Town {townName } was added to the database.");
                        }
                    }
                }

                using (SqlCommand getVillainIdCommand = new SqlCommand(queryGetVillainId, sqlConnection))
                {
                    getVillainIdCommand.Parameters.AddWithValue("@Name", villainName);

                    bool isParsed = int.TryParse(getVillainIdCommand.ExecuteScalar()?.ToString(), out villainId);

                    if (!isParsed)
                    {
                        using (SqlCommand createVillainCommand = new SqlCommand(createVillain, sqlConnection))
                        {
                            createVillainCommand.Parameters.AddWithValue("@Name", townName);

                            createVillainCommand.ExecuteNonQuery();

                            villainId = (int)getVillainIdCommand.ExecuteScalar();

                            Console.WriteLine($"Villain {villainName} was added to the database.");
                        }
                    }
                }

                using (SqlCommand createMinionCommand = new SqlCommand(createMinion, sqlConnection))
                {
                    createMinionCommand.Parameters.Add(new SqlParameter("@Name", minionName));
                    createMinionCommand.Parameters.Add(new SqlParameter("@Age", minionAge));
                    createMinionCommand.Parameters.Add(new SqlParameter("@TownID", townId));
                    createMinionCommand.ExecuteNonQuery();

                    using (SqlCommand getMinionIdCommand = new SqlCommand(getMinionId, sqlConnection))
                    {
                        getMinionIdCommand.Parameters.Add(new SqlParameter("@Name", minionName));
                        minionId = (int)getMinionIdCommand.ExecuteScalar();
                    }
                }

                using (SqlCommand insertIntoMappingTableCommand = new SqlCommand(insertIntoMappingTable, sqlConnection))
                {
                    insertIntoMappingTableCommand.Parameters.Add(new SqlParameter("@Mid", minionId));
                    insertIntoMappingTableCommand.Parameters.Add(new SqlParameter("@Vid", villainId));
                    insertIntoMappingTableCommand.ExecuteNonQuery();

                    Console.WriteLine($"Successfully added {minionName} to be minion of {villainName}.");
                }
            }
        }
    }
}
