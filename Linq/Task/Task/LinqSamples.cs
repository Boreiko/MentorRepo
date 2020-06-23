using System;
using System.Linq;
using SampleSupport;
using Task.Data;

namespace SampleQueries
{
	[Title("LINQ Module")]
	[Prefix("Linq")]
	public class LinqSamples : SampleHarness
	{

		private DataSource dataSource = new DataSource();

		[Category("Restriction Operators")]
		[Title("Where - Task 1")]
		[Description("This sample uses the where clause to find all elements of an array with a value less than 5.")]
		public void Linq1()
		{
			int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

			var lowNums =
				from num in numbers
				where num < 5
				select num;

			Console.WriteLine("Numbers < 5:");
			foreach (var x in lowNums)
			{
				Console.WriteLine(x);
			}
		}

		[Category("Restriction Operators")]
		[Title("Where - Task 2")]
		[Description("This sample return return all presented in market products")]

		public void Linq2()
		{
			var products =
				from p in dataSource.Products
				where p.UnitsInStock > 0
				select p;

			foreach (var p in products)
			{
				ObjectDumper.Write(p);
			}
		}

	    [Category("Restriction Operators")]
		[Title("Task 1")]
		[Description("A list of customers whose total sum of all orders more than X.")]

		public void Linq3()
		{
			int X = 50;
			var customers = 
		    dataSource.Customers.Where(o => o.Orders.Sum(or => or.Total) > X)
				                .Select(cust => new { Adress = cust.Address, Sum = cust.Orders.Sum(od=>od.Total) }) ;

			foreach (var item in customers)
			{
				ObjectDumper.Write(item);				
			}
		}


		[Category("Restriction Operators")]
		[Title("Task 2/1")]
		[Description("A list of suppliers located in the same country and the same city for each customer")]
		public void Linq4()
		{			
			var suppliers = from c in dataSource.Customers
						   from s in dataSource.Suppliers
						   where c.Country == s.Country && c.City == s.City
						   select new { c.CustomerID, s.SupplierName, c.Country, c.City };

			foreach (var item in suppliers)
			{
				ObjectDumper.Write(item);
			}
		}

		[Category("Restriction Operators")]
		[Title("Task 2/2")]
		[Description("A list of suppliers located in the same country and the same city for each customer.")]
		public void Linq5()
		{
			var suppliers = from c in this.dataSource.Customers
							join s in this.dataSource.Suppliers on new { c.Country, c.City } equals
								new { s.Country, s.City }
							select new { c.CustomerID, s.SupplierName, c.City, c.Country };

			foreach (var item in suppliers)
			{
				ObjectDumper.Write(item);
			}

		}


		[Category("Restriction Operators")]
		[Title("Task 3")]
		[Description("All customers who have sum of orders more than X.")]
		public void Linq6()
		{
			int x = 5000;
			var customers = dataSource.Customers.Where(c => c.Orders.Any(o => o.Total > x));

			foreach (var item in customers)
			{
				ObjectDumper.Write(item);
			}

		}

		[Category("Restriction Operators")]
		[Title("Task 4")]
		[Description("A list of customers with an output of the beginning of which month of which year they became customers.")]
		public void Linq7()
		{
			var customers = dataSource.Customers.Where(c => c.Orders.Count() > 1)
				.Select(cust => new { Id = cust.CustomerID, Month = cust.Orders.Min(o => o.OrderDate).Month, Year = cust.Orders.Min(o => o.OrderDate).Year });

			foreach (var item in customers)
			{
				ObjectDumper.Write(item);
			}
		}

		[Category("Restriction Operators")]
		[Title("Task 5")]
		[Description(
		 "The list from previous task sorted by year, month, customer turnover (from maximum to minimum) and customer name")]
		public void Linq8()
		{
			var customers = dataSource.Customers.Where(c => c.Orders.Count() > 1)
					.OrderByDescending(cust => cust.Orders.Min(o => o.OrderDate))
					.ThenByDescending(cust => cust.Orders.Min(o => o.OrderDate).Month)
					.ThenByDescending(cust=> cust.Orders.Min(o => o.OrderDate).Year)
					.ThenByDescending(cust=> cust.CompanyName);
		
			foreach (var item in customers)
			{
				ObjectDumper.Write(item);
			}
		}

		[Category("Restriction Operators")]
		[Title("Task 6")]
		[Description(
		   "A list of customers who have a non-numeric postal code or a region is hasn't been fiiled or the operator code there isn't in the phone.")]
		public void Linq9()
		{		
			var customers = dataSource.Customers.Where(c=> c.PostalCode == null 
			|| c.PostalCode.Any(p=>!char.IsDigit(p))  
			|| string.IsNullOrWhiteSpace(c.Region) 
			|| !c.Phone.StartsWith("("));

			foreach (var c in customers)
			{
     			ObjectDumper.Write(c);
			}
		}

		[Category("Restriction Operators")]
		[Title("Task 7")]
		[Description("A list of products grouped by categories, inside - by stock availability, inside the last group, sorted by cost.")]

		public void Linq10()
		{

		   	var groups = from pr in dataSource.Products
						 group pr by pr.Category
						 into categoryGroup
						 select new
						 {
							 Products = from pr in categoryGroup
										group pr by pr.UnitsInStock
												   into availabilityGroup
										select new
										{
											Products = from pr in availabilityGroup
													   orderby pr.UnitPrice
													   select pr
										}
						 };

			foreach (var categoryGroup in groups)
			{
				foreach (var availabilityGroup in categoryGroup.Products)
				{
					foreach (var product in availabilityGroup.Products)
					{
						ObjectDumper.Write(product);
					}
				}
			}
		}


		[Category("Restriction Operators")]
		[Title("Task 8")]
		[Description("A list of products grouped into 'cheap', 'average price' and 'expensive'.")]
		public void Linq11()
		{
			int cheap = 10;
			int average = 100;

			
			var groups = from p in dataSource.Products
						 group p by p.UnitPrice < cheap ? "Cheap" :
									p.UnitPrice < average ? "Average" : 
									"Expensive";
			     

			foreach (var g in groups)
			{
				ObjectDumper.Write(g.Key);
				foreach (var p in g)
				{
					ObjectDumper.Write(p);
				}
			}
		}

		[Category("Restriction Operators")]
		[Title("Task 9")]
		[Description("Calculation of the average profitability and average intensity of each city.")]
		public void Linq12()
		{
			var cities = from c in dataSource.Customers
						 group c by c.City
						 into cityGroup
						 select new
						 {
							 city = cityGroup.Key,
							 averageProfitability = cityGroup.Average(c => c.Orders.Sum(o => o.Total)),
							 averageIntensity = cityGroup.Average(c => c.Orders.Count())

						 };


			foreach (var c in cities)
			{
				ObjectDumper.Write(c);
			}
		}

		[Category("Restriction Operators")]
		[Title("Task 10-1")]
		[Description("Monthly average customer's activity statistics.")]
		public void Linq13()
		{
			var customers = from c in dataSource.Customers
							select new
							{
								ID = c.CustomerID,
								Months = from o in c.Orders
										 group o by o.OrderDate.Month
										 into monthGroup
										 select new
										 {
											 month = monthGroup.Key,
											 monthActivity = monthGroup.Count()
										 }
							};

			foreach (var c in customers)
			{
				ObjectDumper.Write(c);
				foreach (var m in c.Months)
				{
					ObjectDumper.Write(m);
				}
			}
		}

		[Category("Restriction Operators")]
		[Title("Task 10-2")]
		[Description("Yearly average customer's activity statistics.")]
		public void Linq14()
		{
			var customers = from c in dataSource.Customers
							select new
							{
								ID = c.CustomerID,
								Years = from o in c.Orders
										 group o by o.OrderDate.Year
										 into yearGroup
										 select new
										 {
											 year = yearGroup.Key,
											 yearActivity = yearGroup.Count()
										 }
							};

			foreach (var c in customers)
			{
				ObjectDumper.Write(c);
				foreach (var m in c.Years)
				{
					ObjectDumper.Write(m);
				}
			}
		}

		[Category("Restriction Operators")]
		[Title("Task 10-3")]
		[Description("Yearly and monthly average customer's activity statistics.")]
		public void Linq15()
		{
			var customers = from c in dataSource.Customers
							select new
							{
								ID = c.CustomerID,
								YearMonths = from o in c.Orders
										group o by new { o.OrderDate.Year, o.OrderDate.Month}
										into yearMonthGroup
										select new
										{
											year = yearMonthGroup.Key.Year,
											month = yearMonthGroup.Key.Month,
											yearActivity = yearMonthGroup.Count()
										}
							};

			foreach (var c in customers)
			{
				ObjectDumper.Write(c);
				foreach (var m in c.YearMonths)
				{
					ObjectDumper.Write(m);
				}
			}
		}
	}
}
