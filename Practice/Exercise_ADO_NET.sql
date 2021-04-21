-- Exercises: Introduction to DB Apps

-- 1.	Initial Setup 


CREATE DATABASE MinionsDB 

GO

CREATE TABLE Countries ( [Id] INT PRIMARY KEY IDENTITY, [Name] VARCHAR(50) )

CREATE TABLE Towns([Id] INT PRIMARY KEY IDENTITY,  [Name] VARCHAR(50), [CountryCode] INT FOREIGN KEY REFERENCES Countries(Id) )

CREATE TABLE Minions(  [Id] INT PRIMARY KEY IDENTITY,[Name] VARCHAR(50), [Age] INT,   [TownId] INT FOREIGN KEY REFERENCES Towns(Id))

CREATE TABLE EvilnessFactors(  [Id] INT PRIMARY KEY IDENTITY,  [Name] VARCHAR(30))

-- CHECK ([Name] IN ('super good', 'good', 'bad', 'evil', 'super evil'))

CREATE TABLE Villains ([Id] INT PRIMARY KEY IDENTITY ,[Name] VARCHAR(30),[EvilnessFactorsId] INT FOREIGN KEY REFERENCES EvilnessFactors(Id))

CREATE TABLE MinionsVillains([MinionId] INT FOREIGN KEY REFERENCES Minions(Id), [VillainId] INT FOREIGN KEY REFERENCES Villains(Id),  CONSTRAINT PK_MinionsVillains PRIMARY KEY (MinionId, VillainId) )

INSERT INTO Minions (Name, Age, TownId) VALUES ('Stoyan', 12, 1), ('George', 22, 2), ('Ivan', 25, 4), ('Kiro', 35, 5),('Niki', 25, 3)

INSERT INTO Towns (Name, CountryCode) VALUES ('Plovdiv',1),('Oslo',2),('London',3),('Pasargadae',4),('Athens',5)

INSERT INTO Countries (Name) VALUES ('Bulgaria'), ('Norway'), ('UK'), ('Cyprus'), ('Greece')

INSERT INTO Villains (Name, EvilnessFactorsId) VALUES ('Gru',1),('Ivo',2),('Teo',3),('Sto',4),('Pro',5)

INSERT INTO EvilnessFactors (Name) VALUES ('super good'),('good'),('bad'),('evil'),('super evil')

INSERT INTO MinionsVillains (MinionId,VillainId) VALUES (1,1),(2,2),(3,3),(4,4),(5,5)

-- 2.	Villain Names

GO

SELECT v.[Name], Count(m.Id) AS [Minions Count]
FROM Villains AS v
JOIN MinionsVillains AS mv ON mv.VillainId = v.Id
JOIN Minions AS m ON m.Id = mv.MinionId
GROUP BY v.Name, m.Id
HAVING COUNT(m.Id) > 3
ORDER BY [Minions Count]

-- 3.	Minion Names

GO

SELECT v.Name, m.Name, m.Age
FROM Villains AS v
LEFT JOIN MinionsVillains AS mn ON v.Id = mn.VillainId
LEFT JOIN Minions AS m ON m.Id = mn.MinionId
WHERE v.Id = @Parameter
ORDER BY m.Name

-- Different approach

GO

SELECT ROW_NUMBER() OVER (ORDER BY m.Name) AS [RowNum],
    m.Name,
    m.Age
FROM MinionsVillains AS mv
JOIN Minions AS m ON m.Id = mv.MinionId
WHERE mv.VillainId = @Parameter

-- 4.	Add Minion

-- Minion , Town, Name, Age 
-- @Mname VARCHAR (30), @Mage INT
GO

CREATE PROCEDURE usp_AddTownToDB(@Town VARCHAR(30))
AS
BEGIN TRANSACTION

IF EXISTS (SELECT * FROM Towns WHERE Name = @Town)
BEGIN 
    ROLLBACK;
    THROW 50001, 'Town already in the database',  1
END 
ELSE
BEGIN
INSERT INTO Towns (Name, CountryCode)  VALUES (@Town, 1)
END

COMMIT
GO

EXEC dbo.usp_AddTownToDB Pernik

GO

CREATE PROCEDURE usp_AddVillain(@Vname VARCHAR(30))
AS
BEGIN TRANSACTION

IF EXISTS (SELECT * FROM Villains WHERE Name = @Vname)
BEGIN
    ROLLBACK;
    THROW 50002, 'Villain alreay in the DB', 1
END 
ELSE 
BEGIN 
INSERT INTO Villains (Name, EvilnessFactorId) VALUES (@Vname, 4)
END 

COMMIT 

GO

EXEC dbo.usp_AddVillain @VillainName

GO

CREATE PROCEDURE usp_AddMinion (@Mname VARCHAR(30), @Mage INT, @Town VARCHAR(30))
AS
BEGIN TRANSACTION

DECLARE @TownID INT = (SELECT Id FROM Towns WHERE Name = @Town)

IF @TownID IS NULL
BEGIN
    ROLLBACK;
    THROW 50001, 'Invalid Town ID Minion is not added', 1
END
ELSE 
BEGIN
INSERT INTO Minions (Name, Age, TownId) VALUES (@Mname, @Mage, @TownID)
END

COMMIT

EXEC dbo.usp_AddMinion @Mname, @Age, @Town

GO

CREATE PROCEDURE usp_MakeMinionServant (@Mname VARCHAR(30), @Vname VARCHAR(30))
AS 
BEGIN TRANSACTION

IF NOT EXISTS (SELECT * FROM Minions WHERE Name = @Mname)
BEGIN 
    ROLLBACK;
    THROW 50001, 'Minion is not in the database', 1
END

IF NOT EXISTS (SELECT * FROM Villains WHERE Name = @Vname)
BEGIN 
    ROLLBACK;
    THROW 50001, 'Villain is not in the database', 1
END

IF (SELECT COUNT(Id) FROM Minions WHERE Name = @Mname) > 1
BEGIN
    ROLLBACK;
    THROW 50001, 'More than one Minion with the sme name', 1
END

IF (SELECT COUNT(Id) FROM Minions WHERE Name = @Vname) > 1
BEGIN
    ROLLBACK;
    THROW 50001, 'More than one Villain with the sme name', 1
END

DECLARE @MinionID INT = (SELECT Id FROM Minions WHERE Name = @Mname)
DECLARE @VillainID INT = (SELECT Id FROM Villains WHERE Name = @Vname)

DECLARE @IsConnected VARCHAR(30)  = (SELECT v.Name FROM Villains AS v
                                    JOIN MinionsVillains AS mv ON mv.VillainId = v.Id
                                    WHERE v.Name = @Vname AND mv.MinionId = @MinionID)

IF @IsConnected IS NOT NULL
BEGIN
    ROLLBACK;
    THROW 50001, 'Villain already has this minion for servant', 1
END

INSERT INTO MinionsVillains (MinionId,VillainId) VALUES (@MinionID, @VillainID)

COMMIT 

GO

EXEC dbo.usp_MakeMinionServant @Mname, @Vname

GO

-- 4.	Add Minion - Review

SELECT Id FROM Towns WHERE Name = @Town

INSERT INTO Towns (Name, CountryCode)  VALUES (@Town, 1)

SELECT Id FROM Villains WHERE Name = @Name













 
