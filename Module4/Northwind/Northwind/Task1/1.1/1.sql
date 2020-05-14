--Выбрать в таблице Orders заказы, которые были доставлены после 6 мая 1998 года (колонка ShippedDate) 
--включительно и которые доставлены с ShipVia >= 2. Запрос должен возвращать только колонки OrderID,
--ShippedDate и ShipVia. 



   Select  OrderId  AS 'OrderID', ShippedDate AS 'ShippedDate',ShipVia AS 'ShipVia'
   from Orders
   where ShippedDate > 1998-06-01 AND ShipVia >= 2