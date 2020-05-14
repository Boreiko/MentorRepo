--По таблице Orders найти количество заказов, сделанных каждым продавцом и для каждого покупателя. 
--Необходимо определить это только для заказов, сделанных в 1998 году. 

SELECT 
    EmployeeID, CustomerID, COUNT(OrderID)
FROM Orders
WHERE YEAR(OrderDate) = 1998
GROUP BY EmployeeID,CustomerID
