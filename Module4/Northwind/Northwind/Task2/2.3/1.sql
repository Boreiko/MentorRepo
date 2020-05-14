--Определить продавцов, которые обслуживают регион 'Western' (таблица Region). 
SELECT DISTINCT 
    EmployeeId,FirstName
FROM Employees
        INNER JOIN EmployeeTerritories 
            ON Employees.EmployeeID= EmployeeTerritories.EmployeeID
        INNER JOIN Territories 
            ON EmployeeTerritoriesTerritoryID = TerritoriesTerritoryID
        INNER JOIN Region 
            ON Region.RegionID = Territories.RegionID
WHERE Region.RegionDescription = 'Western';