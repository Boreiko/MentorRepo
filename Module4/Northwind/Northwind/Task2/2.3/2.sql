--Выдать в результатах запроса имена всех заказчиков из таблицы Customers и суммарное количество их заказов из таблицы Orders. 
--Принять во внимание, что у некоторых заказчиков нет заказов, но они также должны быть выведены в результатах запроса. 
--Упорядочить результаты запроса по возрастанию количества заказов.

SELECT ContactName, COUNT(OrderId)   
FROM Customers
    LEFT JOIN Orders
        ON Customers.CustomerId= Orders.CustomerId
GROUP BY Customers.CustomerId, Customers.ContactName
ORDER BY OrdersCount;