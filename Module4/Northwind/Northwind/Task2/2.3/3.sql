--Выбрать всех заказчиков из таблицы Customers, у которых название страны начинается 
--на буквы из диапазона b и g, не используя оператор BETWEEN. 


Select CustomerID,Country from Customers
  Where Country Like 'b%' or Country Like 'g%'
  Order By Country
