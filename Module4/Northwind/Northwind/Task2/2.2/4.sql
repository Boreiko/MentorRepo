--Найти покупателей и продавцов, которые живут в одном городе. Если в городе живут только один или несколько продавцов,
--или только один или несколько покупателей, то информация о таких покупателя и продавцах не должна попадать в результирующий набор.
--Не использовать конструкцию JOIN

SELECT 
  CustomerId,EmployeeID, City     
FROM Customers
CROSS APPLY (SELECT EmployeesT.[EmployeeId] 
                FROM [dbo].[Employees] EmployeesT 
                WHERE EmployeesT.[City] = CustomersT.[City]) EmployeesT;