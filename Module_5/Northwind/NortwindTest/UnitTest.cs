using System;
using System.Collections.Generic;
using System.Linq;
using DeepEqual.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NorthwindDAL;
using NorthwindDAL.Repositories;

namespace NorthwindUnitTest
{
    [TestClass]
    public class NorthwindUnitTest
    {
        const string stringConnection = "Data Source=localhost;Initial Catalog=Northwind;Integrated Security=True";
        const string providerName = "System.Data.SqlClient";
        OrderRepository orderRepository = new OrderRepository(stringConnection, providerName, new ObjectMapper(), new NorthwindDbConnectionFactory());


        [TestMethod]
        public void AddNewOrder()
        {
            var countBeforeAdding = Commands.ExecuteSelectCountOrders();
            orderRepository.AddNew(Commands.GetNewOrder());
            var countAfterAdding = Commands.ExecuteSelectCountOrders();
            Assert.IsTrue(countBeforeAdding + 1 == countAfterAdding);
        }


        [TestMethod]
        public void DeleteOrderByID()
        {
            var LastRowID = Commands.ExecuteSelectMaxOrders();
            var countBeforeDeleting = Commands.ExecuteSelectCountOrders();
            orderRepository.DeleteOrderByID(LastRowID);
            var countAfterDeleting = Commands.ExecuteSelectCountOrders();
            Assert.IsTrue(countBeforeDeleting - 1 == countAfterDeleting);
        }


        [TestMethod]
        public void SetInProgress_OrderDateIsNull_OrderDateHasDate()
        {
            var id = Commands.ExecuteSelectMaxOrdersNewStatus();
            var value = Commands.ExecuteSelectOrderDateValue(id);
            Assert.IsTrue(value.ToString().Equals(String.Empty));
            orderRepository.SetInProgress(id);
            value = Commands.ExecuteSelectOrderDateValue(id);
            Assert.IsNotNull(value);
        }


        [TestMethod]
        public void SetInDone_ShippedDateISNull_ShippedDateHasValue()
        {
            var id = Commands.ExecuteSelectMaxOrdersInProgress();
            var value = Commands.ExecuteSelectShippedDateValue(1);
            Assert.IsTrue(value == null);
            orderRepository.SetInDone(id);
            value = Commands.ExecuteSelectShippedDateValue(id);
            Assert.IsNotNull(value);
        }


        [TestMethod]
        public void ExecuteCustOrderHist_CustOrderHist()
        {
            var ec = Commands.GetTestCustOrderHist();
            var actual = orderRepository.GetCustomerProductDetails("TOMSP");
            Assert.IsTrue(actual.IsDeepEqual(ec));
        }

        [TestMethod]
        public void ExecuteCustOrdersDetail_CustOrdersDetail()
        {
            var ec = Commands.GetTestCustOrdersDetail();
            var actual = orderRepository.GetCustomerOrderDetails(10248);
            Assert.IsTrue(actual.IsDeepEqual(ec));
        }


        [TestMethod]
        public void Update_OrderForUpdateWithID11075_OrderWIthNewValue()
        {
            orderRepository.Update(Commands.GetOrderForUpdate("TestValue1"));
            var actual = Commands.ExecuteSelectShipNameValue(11075);
            Assert.IsTrue(actual.Equals("TestValue1"));
            orderRepository.Update(Commands.GetOrderForUpdate("TestValue2"));
            actual = Commands.ExecuteSelectShipNameValue(11075);
            Assert.IsTrue(actual.Equals("TestValue2"));
        }

        [TestMethod]
        public void GetOrderById_11080_OrderWithID11080()
        {
            var ec = Commands.GetTestOrder();
            var actual = orderRepository.GetOrderById(11080);
            Assert.IsTrue(actual.IsDeepEqual(ec));
        }
    }
}

