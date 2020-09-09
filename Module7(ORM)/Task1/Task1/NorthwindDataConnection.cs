using LinqToDB;
using LinqToDB.Data;
using Task1.models;

namespace Task1
{
    public class NorthwindDataConnection : DataConnection
    {
        public NorthwindDataConnection() : base("Northwind") { }
        public ITable<Category> Categories { get { return GetTable<Category>(); } }
        public ITable<Customer> Customers { get { return GetTable<Customer>(); } }
        public ITable<CustomerCustomerDemo> CustomerCustomerDemos { get { return GetTable<CustomerCustomerDemo>(); } }
        public ITable<CustomerDemographic> CustomerDemographics { get { return GetTable<CustomerDemographic>(); } }
        public ITable<Employee> Employees { get { return GetTable<Employee>(); } }
        public ITable<EmployeeTerritory> EmployeeTerritorys { get { return GetTable<EmployeeTerritory>(); } }
        public ITable<OrderDetail> OrderDetails { get { return GetTable<OrderDetail>(); } }
        public ITable<Order> Order { get { return GetTable<Order>(); } }
        public ITable<Product> Products { get { return GetTable<Product>(); } }
        public ITable<Region> Regions { get { return GetTable<Region>(); } }
        public ITable<Shipper> Shippers { get { return GetTable<Shipper>(); } }
        public ITable<Supplier> Suppliers { get { return GetTable<Supplier>(); } }
        public ITable<Territory> Territories { get { return GetTable<Territory>(); } }
    }
}
