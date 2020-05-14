    CREATE TABLE CreditCards(
        CreditCardId INT NOT NULL PRIMARY KEY IDENTITY(1,1),
        ExpirationDate DATETIME DEFAULT(NULL),
        CardHolderName VARCHAR(200) NOT NULL,
        EmployeeId INT FOREIGN KEY REFERENCES Employees(EmployeeID))

