--Выдать всех заказчиков (таблица Customers), которые не имеют ни одного заказа 
--(подзапрос по таблице Orders). Использовать оператор EXISTS

SELECT CustomerId
FROM Customers
WHERE NOT EXISTS (SELECT OrderId
                    FROM Orders
                    WHERE Orders.CustomerID = CustomersCustomerID);