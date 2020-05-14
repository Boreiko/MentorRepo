--Выбрать в таблице Orders заказы, которые были доставлены после 6 мая 1998 года (ShippedDate) 
--не включая эту дату или которые еще не доставлены. В запросе должны возвращаться только колонки 
--OrderID (переименовать в Order Number) и ShippedDate (переименовать в Shipped Date).
--В результатах запроса возвращать для колонки ShippedDate вместо значений NULL строку ‘Not Shipped’, 
--для остальных значений возвращать дату в формате по умолчанию.


  Select OrderID AS 'Order Number',
   Case
   When ShippedDate is NULL Then 'Not Shipped'
   Else CONVERT(VARCHAR(50), ShippedDate, 0)
   End AS 'Shipped Date'
   from Orders 
   where ShippedDate > 1998-06-01 or ShippedDate is Null
