--По таблице Employees найти для каждого продавца его руководителя.
SELECT 
    EmployeeID, FirstName,
    (SELECT FirstName
     FROM Employees  
     WHERE EmployeeID = ReportsTo) 
FROM Employees