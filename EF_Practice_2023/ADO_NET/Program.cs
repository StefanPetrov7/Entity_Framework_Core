using Microsoft.Data.SqlClient;
using System.Net.Http.Headers;

namespace ADO_NET_Practice_2023
{
    internal class Program
    {
        static void Main(string[] args)
        {

            string connectionString = "Server=.;Integrated Security=true;TrustServerCertificate=true;Database={0}";
            const string MINIONS_DB = "MinionsDB";

            using (var connection = new SqlConnection(string.Format(connectionString, MINIONS_DB)))
            {
                connection.Open();

                // => Create DB, Create Tables and Insert into the tables. => Task 1
                // InitialSetUp(connection); 

                //  => Execute query  => Task 2
                // GetVillainsNames(connection); 

                // => Execute query => Task 3
                // GetMinionsNames(connection);

                // => Adding Minions and Vilains and linking them => Task 4
                // AddMinion(connection);

                // => Task 5 
                // ChangeTownCasingToUpperLetters(connection);

                // => Task 6
                //RemoveVilian(connection);

                // => Task 7
                // PrintAllMinions(connection);

                // => Taks 8
                // UpdateMinions(connection);

                // => Taks 9
                // CreateProcedureIncreaseAge(connection);
                // ExecuteIncreaseAgeProceure(connection);

            };
        }

        private static void ExecuteIncreaseAgeProceure(SqlConnection connection)
        {
            int id = int.Parse(Console.ReadLine());
            string queryExecuteProcedure = "EXEC usp_IncreaseAge @id";
            string printQuery = "SELECT [Name], CONCAT(Age, ' ', 'years old') AS [Age] FROM Minions";

            using (SqlCommand executeProcedure = new SqlCommand(queryExecuteProcedure, connection))
            {
                executeProcedure.Parameters.AddWithValue("@id", id);
                executeProcedure.ExecuteNonQuery();
            }

            using (SqlCommand printResult = new SqlCommand(printQuery, connection))
            {
                using (SqlDataReader reader = printResult.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader[0]} - {reader[1]}");
                    }
                }
            }
        }

        private static void CreateProcedureIncreaseAge(SqlConnection connection)
        {
            string queryProcedure = @"CREATE PROCEDURE usp_IncreaseAge (@id INT)
                                    AS
                                    UPDATE Minions
                                    SET Age += 1
                                    WHERE id = @id";


            using (SqlCommand createProcedure = new SqlCommand(queryProcedure, connection))
            {
                createProcedure.ExecuteNonQuery();
            }
        }

        private static void UpdateMinions(SqlConnection connection)
        {
            int[] data = Console.ReadLine().Split(" ").Select(int.Parse).ToArray();
            string updateQuery = "EXEC usp_UpdateMinionsAgeAndUpperCase @id";
            string queryPrintNameAge = "SELECT [Name], Age FROM Minions";

            using (SqlCommand update = new SqlCommand(updateQuery, connection))
            {
                for (int i = 0; i < data.Length; i++)
                {
                    int id = data[i];
                    update.Parameters.AddWithValue("@id", id);
                    update.ExecuteNonQuery();
                }
            }

            using (SqlCommand printMinions = new SqlCommand(queryPrintNameAge, connection))
            {
                using (SqlDataReader reader = printMinions.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader[0]} {reader[1]}");
                    }
                }
            }
        }

        private static void PrintAllMinions(SqlConnection connection)
        {
            string query = "SELECT [Name] FROM Minions";
            List<string> minions = new List<string>();

            using (SqlCommand printMinions = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = printMinions.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        minions.Add((string)reader[0]);
                    }
                }
            }

            while (minions.Count > 0)
            {
                Console.WriteLine(minions[0]);
                minions.RemoveAt(0);

                if (minions.Count > 0)
                {
                    Console.WriteLine(minions[minions.Count - 1]);
                    minions.RemoveAt(minions.Count - 1);
                }
            }
        }

        private static void RemoveVilian(SqlConnection connection)
        {
            int id = int.Parse(Console.ReadLine());
            string queryIsVilianExists = "SELECT [Name] FROM Villains WHERE Id = @id";
            string queryDeleteVillainFromReferencedTable = "DELETE FROM MinionsVillains WHERE VillainId = @id";
            string queryDeleteVillain = "DELETE FROM Villains WHERE Id = @id";
            string name;

            using (SqlCommand checkIfExist = new SqlCommand(queryIsVilianExists, connection))
            {
                checkIfExist.Parameters.AddWithValue("@id", id);
                name = (string)checkIfExist.ExecuteScalar();

                if (name == null)
                {
                    Console.WriteLine("No such villain was found.");
                    return;
                }
            }

            var countDeletedMinions = 0;

            if (countDeletedMinions > 0)
            {
                using (SqlCommand deleteFromLinkTable = new SqlCommand(queryDeleteVillainFromReferencedTable, connection))
                {
                    deleteFromLinkTable.Parameters.AddWithValue("@id", id);
                    countDeletedMinions = deleteFromLinkTable.ExecuteNonQuery();
                }
            }

            using (SqlCommand finalDelete = new SqlCommand(queryDeleteVillain, connection))
            {
                finalDelete.Parameters.AddWithValue("@id", id);
                finalDelete.ExecuteNonQuery();
            }

            Console.WriteLine($"{name} was deleted.");
            Console.WriteLine($"{countDeletedMinions} minions were released.");

        }

        private static void ChangeTownCasingToUpperLetters(SqlConnection connection)
        {
            string town = Console.ReadLine();

            string query = @"SELECT c.[Name] AS [Country], 
		                    UPPER(t.[Name]) AS[Town], 
		                    ROW_NUMBER() OVER (ORDER BY t.[Name]) AS [Number]
                            FROM Countries AS c
                            LEFT JOIN Towns AS t on c.Id = t.CountryCode
                            WHERE c.Name = @Town
                            GROUP BY C.[Name],t.[Name]";

            using (SqlCommand upperCaseTowns = new SqlCommand(query, connection))
            {
                upperCaseTowns.Parameters.AddWithValue("@Town", town);

                using (var reader = upperCaseTowns.ExecuteReader())
                {
                    List<string[]> rows = new List<string[]>();

                    if (reader.HasRows == false)
                    {
                        Console.WriteLine("No town names were affected.");
                    }
                    else
                    {
                        while (reader.Read())
                        {
                            rows.Add(new string[] { reader[0].ToString(), reader[1].ToString(), reader[2].ToString() });
                        }

                        string[] towns = new string[rows.Count];
                        for (int i = 0; i < rows.Count; i++)
                        {
                            towns[i] = rows[i][1];
                        }
                        ;
                        if (towns[0] == string.Empty)
                        {
                            Console.WriteLine("No town names were affected.");
                        }
                        else
                        {
                            Console.WriteLine($"{rows.Count} town names were affected");
                            Console.WriteLine($"[{string.Join(" ,", towns)}]");
                        }
                    }
                }
            }
        }

        private static void AddMinion(SqlConnection connection)
        {
            string[] minionInfo = Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries).ToArray();
            string[] villainInfo = Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries).ToArray();

            string vName = villainInfo[1];
            string mName = minionInfo[1];
            int mAge = int.Parse(minionInfo[2]);
            string mTown = minionInfo[3];

            string queryAddTown = "EXEC dbo.sp_AddTownToDB @Town";

            using (SqlCommand addTownCommand = new SqlCommand(queryAddTown, connection))
            {
                addTownCommand.Parameters.AddWithValue("@Town", mTown);

                try
                {
                    addTownCommand.ExecuteNonQuery();
                    Console.WriteLine($"Town {mTown} was added to the Database");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }

            string queryAddVillainToDB = "EXEC dbo.usp_AddVillain @vName";

            using (SqlCommand addVillainToDB = new SqlCommand(queryAddVillainToDB, connection))
            {
                addVillainToDB.Parameters.AddWithValue("@vName", vName);

                try
                {
                    addVillainToDB.ExecuteNonQuery();
                    Console.WriteLine($"Villain {vName} was added to the database.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            string queryAddMinion = "EXEC dbo.usp_AddMinionToDB @Mname, @Age, @Town";

            using (SqlCommand addMinionCommand = new SqlCommand(queryAddMinion, connection))
            {
                addMinionCommand.Parameters.AddWithValue("@Mname", mName);
                addMinionCommand.Parameters.AddWithValue("@Age", mAge);
                addMinionCommand.Parameters.AddWithValue("@Town", mTown);

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

            string queryAddMinionToVIllain = "EXEC usp_AddMinionAsServant @MName, @VName";

            using (SqlCommand addMinionToVillain = new SqlCommand(queryAddMinionToVIllain, connection))
            {
                addMinionToVillain.Parameters.AddWithValue("@MName", mName);
                addMinionToVillain.Parameters.AddWithValue("@VName", vName);

                try
                {
                    addMinionToVillain.ExecuteNonQuery();
                    Console.WriteLine($"Successfully added {mName} to be minion of {vName}.");

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private static void GetMinionsNames(SqlConnection connection)
        {
            int idValue = int.Parse(Console.ReadLine());
            string key = "@id";
            string villainNameQuery = @"SELECT [Name] FROM Villains WHERE ID = @id";

            string minionsQuery = @"SELECT ROW_NUMBER() OVER(ORDER BY m.Name) as RowNum,
                                         m.Name, 
                                         m.Age
                                         FROM MinionsVillains AS mv
                                         JOIN Minions As m ON mv.MinionId = m.Id
                                         WHERE mv.VillainId = @Id
                                         ORDER BY m.Name";

            using (var firstCommand = new SqlCommand(villainNameQuery, connection))
            {
                firstCommand.Parameters.AddWithValue(key, idValue);
                var result = firstCommand.ExecuteScalar();

                if (result == null)
                {
                    result = $"No villain with ID {idValue} exists in the database.";
                    Console.WriteLine(result);
                }
                else
                {
                    Console.WriteLine($"Villain: {result}");

                    List<string> rows = new List<string>();

                    using (var seconCommand = new SqlCommand(minionsQuery, connection))
                    {
                        seconCommand.Parameters.AddWithValue(key, idValue);

                        using (var reader = seconCommand.ExecuteReader())
                        {
                            if (reader.HasRows != false)
                            {
                                while (reader.Read())
                                {
                                    string row = $"{reader[0]} {reader[1]} {reader[2]} ";
                                    rows.Add(row);
                                }
                            }
                        }
                    }

                    if (rows.Count == 0)
                    {
                        Console.WriteLine("No mininons");
                    }
                    else
                    {
                        Console.WriteLine(String.Join("\n", rows));
                    }
                }
            }
        }

        private static void GetVillainsNames(SqlConnection connection)
        {
            string querySelectVillains = @"SELECT v.[Name], COUNT(v.[Name])
                                                    FROM Villains AS v
                                                    LEFT JOIN MinionsVillains AS mv ON v.ID = mv.VillainID
                                                    LEFT JOIN Minions AS m ON mv.MinionID = m.ID
                                                    GROUP BY v.[Name], m.[Name]";

            using (var command = new SqlCommand(querySelectVillains, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var columnName = reader[0];
                        var columnCount = reader[1];

                        Console.WriteLine($"{columnName} - {columnCount}");
                    }
                }
            }
        }

        private static void InitialSetUp(SqlConnection connection)
        {
            var createDataBase = "CREATE DATABASE MinionsDB";

            CreateDB(connection, createDataBase);  // => Create DB 

            var createTableStatements = GetCreatedTableStatements(); // => query strings to create tables 

            foreach (var query in createTableStatements)  // => executing Nonequery to create the tables 
            {
                ExecuteNoneQuery(connection, query);
            }

            var insertStatements = InsertTableStatements();  // => query strings to insert in the tables 

            foreach (var tableInsert in insertStatements)  // => execute nonequery to process the insert 
            {
                ExecuteNoneQuery(connection, tableInsert);
            }
        }

        private static void CreateDB(SqlConnection connection, string query)
        {
            using var command = new SqlCommand(query, connection);
            command.ExecuteNonQuery();
        }


        private static void ExecuteNoneQuery(SqlConnection connection, string query)
        {
            using var command = new SqlCommand(query, connection);
            command.ExecuteNonQuery();
        }

        private static string[] GetCreatedTableStatements()
        {
            return new string[]
             {
                 "CREATE TABLE Countries([ID] INT PRIMARY KEY IDENTITY,  [Name] NVARCHAR(50) NOT NULL)",

                 "CREATE TABLE Towns ([ID] INT PRIMARY KEY IDENTITY," +
                "\t[Name] NVARCHAR(50) NOT NULL,[CountryCode] INT, CONSTRAINT FK_TwonsCountries FOREIGN KEY (CountryCode) REFERENCES Countries(ID))",

                 "CREATE TABLE Minions ([ID] INT PRIMARY KEY IDENTITY, [Name] NVARCHAR(50) NOT NULL, [Age] INT," +
                "\t[TownID] INT CONSTRAINT FK_MinionsTowns \tFOREIGN KEY (TownID)REFERENCES Towns(ID))",

                 "CREATE TABLE EvilnessFactors([ID] INT PRIMARY KEY IDENTITY, [Name] NVARCHAR(50))",

                 "CREATE TABLE Villains([ID] INT PRIMARY KEY IDENTITY,[Name] NVARCHAR(50) NOT NULL,[EvilnessFactorID] INT, CONSTRAINT FK_VillainsEvilnessFactors" +
                "\tFOREIGN KEY (EvilnessFactorID)REFERENCES EvilnessFactors(ID) ) ",

                 "CREATE TABLE MinionsVillains(MinionID INT, VillainID INT,CONSTRAINT PK_MinionVIllain PRIMARY KEY (MinionID, VillainID)," +
                "CONSTRAINT FK_MinionID FOREIGN KEY (MinionID) REFERENCES Minions(ID),CONSTRAINT FK_VillainID FOREIGN KEY (VillainID) REFERENCES Villains(ID))\r\n",

             };

        }

        private static string[] InsertTableStatements()
        {
            return new string[]
            {
               "INSERT INTO Countries ([Name]) VALUES ('Bulgaria'), ('Italy'), ('Germany'), ('Greece'), ('UK')",
               "INSERT INTO Towns ([Name], CountryCode) VALUES ('Plovdiv',1), ('Rome',2),('Berlin', 3),('Atheens',4), ('London', 5)",
               "INSERT INTO Minions VALUES ('Stoyan', 12, 1), ('GEorge', 22, 2), ('Ivan', 22, 4),('Kiro',32,3),('Niki',25,5)",
               "INSERT INTO EvilnessFactors VALUES ('Super good'), ('Bad'), ('Insane'), ('Hero'), ('Evil')",
               "INSERT INTO Villains VALUES ('Gru', 1), ('Iskren', 2), ('Viks', 3), ('Lub', 4), ('Packo', 5)",
               "INSERT INTO MinionsVillains VALUES (1,1), (2,2), (3,3), (4,4), (5,5)"
            };
        }
    }
}