using LinqToDB;
using LinqToDB.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Task1.models;

namespace Task1
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var connection = new NorthwindDataConnection())
            {
                ListOfProductsWithCategoryAndSupplier(connection);
                EmployeesWithRegion(connection);
                EmployeesCountByRegion(connection);
                EmployeesByShippers(connection);
               
                // Task 3 
                
                AddNewEpmloyeeWithRegion(connection, new Employee { FirstName = "Kirill", LastName = "Boreiko" }, 1);
                MoveProcuctToNewCategory(connection, 1, 2);
                AddProducts(connection, new List<Product>
                    {
                    new Product
                    {
                        ProductName = "TestProduct1",
                        Category = new Category {CategoryName = "Seafood"},
                        Supplier = new Supplier {CompanyName = "TestCompanyName1"}
                    },
                    new Product
                    {
                        ProductName = "TestProduct2",
                        Category = new Category {CategoryName = "Seafood"},
                        Supplier = new Supplier {CompanyName = "TestCompanyName2"}
                    }
                    });
                ReplaceProductInNotShippedOrder(connection);
                Console.Read();
            }

        }
        static void ListOfProductsWithCategoryAndSupplier(NorthwindDataConnection connection)
        {
            var productWCWS = connection.Products.LoadWith(p => p.Category).LoadWith(p => p.Supplier);

            Console.WriteLine("Products:");

            foreach (var product in productWCWS)           
                Console.WriteLine($"Name:{product.ProductName}, Category:{product.Category.CategoryName}, Supplier:{product.Supplier.CompanyName}");
            
        }
        static void EmployeesWithRegion(NorthwindDataConnection connection)
        {
            var employees = from e in connection.Employees
                            join et in connection.EmployeeTerritorys on e.EmployeeID equals et.EmployeeID into eet
                            from t in eet.DefaultIfEmpty()
                            join et in connection.Territories on t.TerritoryID equals et.TerritoryID into ter
                            from te in ter.DefaultIfEmpty()
                            join re in connection.Regions on te.RegionID equals re.RegionID into reg
                            from empReg in reg.DefaultIfEmpty()
                            select new { e.LastName, empReg };

            Console.WriteLine("Employees");

            foreach (var employee in employees)            
                Console.WriteLine($"Last name: {employee.LastName} Employee region: {employee.empReg.RegionDescription}");
            
        }
        static void EmployeesCountByRegion(NorthwindDataConnection connection)
        {
            var EmployeesCountWithRegion = from r in connection.Regions
                                      join t in connection.Territories on r.RegionID equals t.RegionID into rts
                                      from rt in rts.DefaultIfEmpty()
                                      join et in connection.EmployeeTerritorys on rt.TerritoryID equals et.TerritoryID into emters
                                      from emter in emters.DefaultIfEmpty()
                                      select new { Region = r.RegionDescription, EmpID = emter.EmployeeID };

            var regionCount = from row in EmployeesCountWithRegion.Distinct()
                           group row by row.Region into g
                           select new { RegionName = g.Key, EmpCount = g.Count(e => e.EmpID != 0) };

            foreach (var item in regionCount.ToList())           
                Console.WriteLine($"Region name: {item.RegionName} Count: {item.EmpCount}");
            
        }
        static void EmployeesByShippers(NorthwindDataConnection connection)
        {
            var employeesWithShipper = (from e in connection.Employees
                                  join o in connection.Order on e.EmployeeID equals o.EmployeeID into eos
                                  from eo in eos.DefaultIfEmpty()
                                  join s in connection.Shippers on eo.Shippers.ShipperID equals s.ShipperID into zl
                                  from z in zl.DefaultIfEmpty()
                                  select new { e.EmployeeID, z.CompanyName }).Distinct().OrderBy(t => t.EmployeeID);
            
            foreach (var record in employeesWithShipper.ToList())           
                Console.WriteLine($"Employee ID: {record.EmployeeID} Shipper: {record.CompanyName}");           
        }

        // Task 3     
        static void AddNewEpmloyeeWithRegion(NorthwindDataConnection connection, Employee emp, int regionID)
        {
            try
            {
                connection.BeginTransaction();
                var newID = Convert.ToInt32(connection.InsertWithIdentity(emp));

                Console.WriteLine($"Tring add new employee with id {newID}");
                
                connection.Territories.Where(x => x.RegionID == regionID)
                    .Insert(connection.EmployeeTerritorys,
                    t => new EmployeeTerritory { EmployeeID = newID, TerritoryID = t.TerritoryID });

                connection.CommitTransaction();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                connection.RollbackTransaction();
            }
        }
        static void MoveProcuctToNewCategory(NorthwindDataConnection connection, int oldIDCategory, int newIDCategory)
        {
            var countProductBefore = connection.Products.Where(x => x.CategoryID == newIDCategory).Count();

            Console.WriteLine($"Count of products with category {newIDCategory} is {countProductBefore}");

            connection.Products.Where(x => x.CategoryID == oldIDCategory).Set(x => x.CategoryID, newIDCategory).Update();

            var countProductAfter = connection.Products.Where(x => x.CategoryID == newIDCategory).Count();
            Console.WriteLine($"Count of products with category {newIDCategory} is {countProductAfter}");
        }
       
        static void AddProducts(NorthwindDataConnection connection, List<Product> products)
        {
            try
            {
                connection.BeginTransaction();

                foreach (var product in products)
                {
                    var category = connection.Categories.FirstOrDefault(c => c.CategoryName == product.Category.CategoryName);
                    product.CategoryID = category?.CategoryID ?? Convert.ToInt32(connection.InsertWithIdentity(
                                             new Category
                                             {
                                                 CategoryName = product.Category.CategoryName
                                             }));
                    var supplier = connection.Suppliers.FirstOrDefault(s => s.CompanyName == product.Supplier.CompanyName);
                    product.SupplierID = supplier?.SupplierID ?? Convert.ToInt32(connection.InsertWithIdentity(
                                             new Supplier
                                             {
                                                 CompanyName = product.Supplier.CompanyName
                                             }));
                }

                connection.BulkCopy(products);
                connection.CommitTransaction();

                Console.WriteLine("Products added succesfully");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                connection.RollbackTransaction();
            }
        }
        static void ReplaceProductInNotShippedOrder(NorthwindDataConnection connection)
        {
            var orderDetails = connection.OrderDetails.LoadWith(od => od.OrderDetailsOrder)
                                .Where(od => od.OrderDetailsOrder.ShippedDate == null).ToList();

            foreach (var orderDetail in orderDetails)
            {
                Console.WriteLine($"Product ID = {orderDetail.ProductID}");
                Console.WriteLine($"Order ID = {orderDetail.OrderID}");

                connection.OrderDetails.LoadWith(od => od.OrderDetailsProduct).Update(od => od.ProductID == orderDetail.ProductID
                && od.OrderID == orderDetail.OrderID, od => new OrderDetail
                {
                    ProductID = connection.Products.First(p => !connection.OrderDetails.Where(t => t.OrderID == od.OrderID)
                         .Any(t => t.ProductID == p.ProductID) && p.CategoryID == od.OrderDetailsProduct.CategoryID).ProductID
                });

            }    
            orderDetails = connection.OrderDetails.LoadWith(od => od.OrderDetailsOrder)
                           .Where(od => od.OrderDetailsOrder.ShippedDate == null).ToList();

            foreach (var orderDetail in orderDetails)
            {
                Console.WriteLine($"Product ID = {orderDetail.ProductID}");
                Console.WriteLine($"Order ID = {orderDetail.OrderID}");
            }
        }
    }
}
