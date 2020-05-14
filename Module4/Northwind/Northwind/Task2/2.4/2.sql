--Выдать всех продавцов, которые имеют более 150 заказов. Использовать вложенный SELECT

SELECT EmployeeID
FROM Employees
WHERE (SELECT COUNT(OrderID) 
        FROM Orders
        WHERE Orders.EmployeeID = Employees.EmployeeID) > 150;