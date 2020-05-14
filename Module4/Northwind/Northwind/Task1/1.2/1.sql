--Выбрать из таблицы Customers всех заказчиков, проживающих в USA и Canada. 
--Запрос сделать с только помощью оператора IN. Возвращать колонки с именем
--пользователя и названием страны в результатах запроса. Упорядочить результаты 
--запроса по имени заказчиков и по месту проживания.

select ContactName,Country from Customers
Where Country in ('USA','CANADA')
Order By Country , ContactName
