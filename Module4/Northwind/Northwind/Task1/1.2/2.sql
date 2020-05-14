--Выбрать из таблицы Customers всех заказчиков, не проживающих в USA и Canada. 
--Запрос сделать с помощью оператора IN. Возвращать колонки с именем пользователя 
--и названием страны в результатах запроса. Упорядочить результаты запроса по имени заказчиков.

select ContactName,Country from Customers
Where Country Not in ('USA','CANADA')
Order By ContactName
