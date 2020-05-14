IF NOT EXISTS (SELECT * FROM SYS.TABLES WHERE [Name] = 'CreditCards')
    CREATE TABLE CreditCards(
        CreditCardId INT NOT NULL PRIMARY KEY IDENTITY(1,1),
        ExpirationDate DATETIME DEFAULT(NULL),
        CardHolderName VARCHAR(100) NOT NULL,
        EmployeeId INT FOREIGN KEY REFERENCES Employees(EmployeeID))
