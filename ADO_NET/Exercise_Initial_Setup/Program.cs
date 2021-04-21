namespace Exercise_Initial_Setup
{
    using System;
    using Microsoft.Data.SqlClient;

    public class Program
    {
        const string sqlConnectionString = "Data Source=localhost;Database={0} ;User Id=sa;Password=Password123@jkl#";

        public static void Main(string[] args)
        {

            using (var sqlConnection = new SqlConnection(string.Format(sqlConnectionString, "master")))
            {
                sqlConnection.Open();

                string createDb = "CREATE DATABASE MinionsDB";

                try
                {
                    ExecuteNonQuery(sqlConnection, createDb);
                    Console.WriteLine("Database Minions created successfully");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Database was not created");
                    Console.WriteLine(ex.Message);
                }

            }

            using (var sqlConnection = new SqlConnection(string.Format(sqlConnectionString, "MinionsDB")))
            {
                sqlConnection.Open();

                string[] createTableStatement = GetCreateTableStatement();

                try
                {
                    foreach (var query in createTableStatement)
                    {
                        ExecuteNonQuery(sqlConnection, query);
                        Console.WriteLine("Tables was created successfully");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("There was an error creating tables!");
                    Console.WriteLine(ex.Message);
                }
            }

            using (var sqlConnection = new SqlConnection(string.Format(sqlConnectionString, "MinionsDB")))
            {
                sqlConnection.Open();

                string[] insertStatements = GetInsertDataStatement();

                try
                {
                    foreach (var query in insertStatements)
                    {
                        ExecuteNonQuery(sqlConnection, query);
                        Console.WriteLine("Data inserted successfully!");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("There was an error inserting data into tables!");
                    Console.WriteLine(ex.Message);
                }

            }

        }

        private static void ExecuteNonQuery(SqlConnection sqlConnection, string query)
        {
            using (SqlCommand command = new SqlCommand(query, sqlConnection))
            {
                var rowsAffected = command.ExecuteNonQuery();
            }
        }

        private static string[] GetCreateTableStatement()
        {
            string[] tables = new string[]
            {
                "CREATE TABLE Countries ( [Id] INT PRIMARY KEY IDENTITY, [Name] VARCHAR(50) )",

                "CREATE TABLE Towns([Id] INT PRIMARY KEY IDENTITY,  " +
                "[Name] VARCHAR(50), [CountryCode] INT FOREIGN KEY REFERENCES Countries(Id) )",

                "CREATE TABLE Minions([Id] INT PRIMARY KEY IDENTITY,[Name] VARCHAR(50), " +
                "[Age] INT,   [TownId] INT FOREIGN KEY REFERENCES Towns(Id))",

                "CREATE TABLE EvilnessFactors(  [Id] INT PRIMARY KEY IDENTITY,  [Name] VARCHAR(30))",

                "CREATE TABLE Villains ([Id] INT PRIMARY KEY IDENTITY ,[Name] VARCHAR(30)," +
                "[EvilnessFactorsId] INT FOREIGN KEY REFERENCES EvilnessFactors(Id))",

                "CREATE TABLE MinionsVillains([MinionId] INT FOREIGN KEY REFERENCES Minions(Id), " +
                "[VillainId] INT FOREIGN KEY REFERENCES Villains(Id),  " +
                "CONSTRAINT PK_MinionsVillains PRIMARY KEY (MinionId, VillainId) )"
            };

            return tables;
        }

        private static string[] GetInsertDataStatement()
        {
            string[] insertValues = new string[]
            {
                "INSERT INTO Countries (Name) VALUES ('Bulgaria'), ('Norway'), ('UK'), ('Cyprus'), ('Greece')",

                "INSERT INTO Towns (Name, CountryCode) " +
                "VALUES ('Plovdiv',1),('Oslo',2),('London',3),('Pasargadae',4),('Athens',5)",

                "INSERT INTO Minions (Name, Age, TownId) " +
                "VALUES ('Stoyan', 12, 1), ('George', 22, 2), ('Ivan', 25, 4), ('Kiro', 35, 5),('Niki', 25, 3)",

                 "INSERT INTO EvilnessFactors (Name) VALUES ('super good'),('good'),('bad'),('evil'),('super evil')",

                "INSERT INTO Villains (Name, EvilnessFactorsId) VALUES ('Gru',1),('Ivo',2),('Teo',3),('Sto',4),('Pro',5)",

                "INSERT INTO MinionsVillains (MinionId,VillainId) VALUES (1,1),(2,2),(3,3),(4,4),(5,5)"

            };

            return insertValues;
        }
    }
}
