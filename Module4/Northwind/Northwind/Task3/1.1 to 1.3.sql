EXEC sp_rename 'Region', 'Regions';  

Alter table [Customers]
Add FoundingDate DATETIME