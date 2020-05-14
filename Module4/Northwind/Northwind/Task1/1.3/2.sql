--Выбрать всех заказчиков из таблицы Customers, у которых название страны начинается на буквы из диапазона b и g. 
--Использовать оператор BETWEEN. Проверить, что в результаты запроса попадает Germany. Запрос должен возвращать 
--только колонки CustomerID и Country и отсортирован по Country.


Select CustomerID,Country from Customers
  Where Country BETWEEN 'b' and 'h'
  Order By Country
