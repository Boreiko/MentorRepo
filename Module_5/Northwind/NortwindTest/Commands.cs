using NorthwindDAL;
using NorthwindDAL.Models;
using NorthwindDAL.Repositories;
using System;
using System.Collections.Generic;


namespace NorthwindUnitTest
{
    public class Commands
    {
        const string stringConnection = "data source=localhost;Initial Catalog=Northwind;Integrated Security=True";
        const string providerName = "System.Data.SqlClient";
        static OrderRepository orderRepository = new OrderRepository(stringConnection, providerName, new ObjectMapper()
            , new NorthwindDbConnectionFactory());

        static Commands()
        {
            OrderRepository orderRepository = new OrderRepository(stringConnection, providerName, new ObjectMapper(),
                new NorthwindDbConnectionFactory());
        }
        public static int ExecuteSelectCountOrders()
        {
            int count = 0;
            var connection = orderRepository.connectionDB.CreateConnection();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT COUNT(OrderID) FROM Orders";
                count = (int)command.ExecuteScalar();
            }
            connection.Close();
            return count;
        }
        public static int ExecuteSelectMaxOrders()
        {
            int count = 0;
            var connection = orderRepository.connectionDB.CreateConnection();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT MAX(OrderID) FROM Orders";
                count = (int)command.ExecuteScalar();
            }
            connection.Close();
            return count;
        }
        public static int ExecuteSelectMaxOrdersNewStatus()
        {
            int id = 0;
            var connection = orderRepository.connectionDB.CreateConnection();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT MAX(OrderID) FROM Orders WHERE OrderDate is null";
                id = (int)command.ExecuteScalar();
            }
            connection.Close();
            return id;
        }
        public static int ExecuteSelectOrderDateByID()
        {
            int id = 0;
            var connection = orderRepository.connectionDB.CreateConnection();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT MAX(OrderID) FROM Orders WHERE OrderDate is null";
                id = (int)command.ExecuteScalar();
            }
            connection.Close();
            return id;
        }
        public static Order GetNewOrder()
        {
            return new Order()
            {
                OrderDate = new DateTime(2010, 1, 1),
                ShipName = "NewName",
                ShipAddress = "NewAdress",
                Details = new List<OrderDetail>
                {
                    new OrderDetail
                    {
                        Product = new Product{ProductID = 11},
                        Quantity = 10,
                        UnitPrice = 16
                    },
                    new OrderDetail
                    {
                        Product = new Product{ProductID = 10},
                        Quantity = 10,
                        UnitPrice = 25
                    },
                }
            };
        }

        public static object ExecuteSelectOrderDateValue(int id)
        {
            object value = null;
            var connection = orderRepository.connectionDB.CreateConnection();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT OrderDate FROM Orders WHERE OrderID = {id}";
                value = command.ExecuteScalar();
            }
            connection.Close();
            return value;
        }

        public static int ExecuteSelectMaxOrdersInProgress()
        {
            int id = 0;
            var connection = orderRepository.connectionDB.CreateConnection();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT MAX(OrderID) FROM Orders WHERE OrderDate is not null and ShippedDate is null";
                id = (int)command.ExecuteScalar();
            }
            connection.Close();
            return id;
        }

        public static object ExecuteSelectShippedDateValue(int id)
        {
            object value = null;
            var connection = orderRepository.connectionDB.CreateConnection();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT ShippedDate FROM Orders WHERE OrderID = {id}";
                value = command.ExecuteScalar();
            }
            connection.Close();
            return value;
        }

        public static IEnumerable<CustomerProductDetail> GetTestCustOrderHist()
        {
            return new List<CustomerProductDetail>
            {
                new CustomerProductDetail {ProductName = "Filo Mix", Total=15},
                new CustomerProductDetail {ProductName = "Gnocchi di nonna Alice", Total=28},
                new CustomerProductDetail {ProductName = "Gorgonzola Telino", Total=3},
                new CustomerProductDetail {ProductName = "Guaraná Fantástica", Total=20},
                new CustomerProductDetail {ProductName = "Jack's New England Clam Chowder", Total=14},
                new CustomerProductDetail {ProductName = "Manjimup Dried Apples", Total=40},
                new CustomerProductDetail {ProductName = "Maxilaku", Total=40},
                new CustomerProductDetail {ProductName = "Ravioli Angelo", Total=15},
                new CustomerProductDetail {ProductName = "Sasquatch Ale", Total=30},
                new CustomerProductDetail {ProductName = "Teatime Chocolate Biscuits", Total=39},
                new CustomerProductDetail {ProductName = "Tofu", Total=9}
            };
        }

        public static IEnumerable<CustomerOrdersDetail> GetTestCustOrdersDetail()
        {
            return new List<CustomerOrdersDetail>
            {
                new CustomerOrdersDetail {ProductName = "Queso Cabrales", UnitPrice=14, Quantity = 12, Discount = 0, ExtendedPrice = 168},
                new CustomerOrdersDetail {ProductName = "Singaporean Hokkien Fried Mee", UnitPrice=Convert.ToDecimal(9.8), Quantity = 10, Discount = 0, ExtendedPrice = Convert.ToDecimal(98.0000)},
                new CustomerOrdersDetail {ProductName = "Mozzarella di Giovanni", UnitPrice=Convert.ToDecimal(34.8), Quantity = 5, Discount = 0, ExtendedPrice = 174}
            };
        }
       
        public static Order GetOrderForUpdate(string ShipName)
        {
            return new Order()
            {
                OrderID = 11075,
                OrderDate = null,
                ShipName = ShipName,
                ShipAddress = "Adress",
                Details = new List<OrderDetail>
                {
                    new OrderDetail
                    {
                        Product = new Product{ProductID = 7},
                        Quantity = 5,
                        UnitPrice = 20
                    },
                    new OrderDetail
                    {
                        Product = new Product{ProductID = 6},
                        Quantity = 5,
                        UnitPrice = 25
                    },
                }
            };
        }

        public static object ExecuteSelectShipNameValue(int id)
        {
            object value = null;
            var connection = orderRepository.connectionDB.CreateConnection();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"SELECT ShipName FROM Orders  where OrderID = {id}";
                value = command.ExecuteScalar();
            }
            connection.Close();
            return value;
        }

        public static Order GetTestOrder()
        {
            return new Order()
            {
                OrderID = 11080,
                ShipName = "TestName",
                ShipAddress = "TestAddress",
                Details = new List<OrderDetail>()

            };
        }
    }
}
