using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Task.DB;
using System.Runtime.Serialization;
using System.Linq;
using Task.Surrogate;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using Task.TestHelpers;

namespace Task
{
    [TestClass]
    public class SerializationSolutions
    {
        Northwind dbContext;

        [TestInitialize]
        public void Initialize()
        {
            dbContext = new Northwind();
        }
        public SerializationTester<IEnumerable<T>> SetSerializer<T>(CustomContext sC)
        {
            var serializer = new NetDataContractSerializer(new StreamingContext(StreamingContextStates.All, sC));
            var xmlSerializer = new XmlDataContractSerializer<IEnumerable<T>>(serializer);
            return new SerializationTester<IEnumerable<T>>(xmlSerializer);          
        }
        public SerializationTester<IEnumerable<T>> GetTester<T>(XmlObjectSerializer xmlSerializer)
        {
            var serializer = new XmlDataContractSerializer<IEnumerable<T>>(xmlSerializer);
            return new SerializationTester<IEnumerable<T>>(serializer);
        }

        [TestMethod]
        public void SerializationCallbacks()
        {
            dbContext.Configuration.ProxyCreationEnabled = false;

            var serializationContext = new CustomContext
            {
                ObjectContext = (dbContext as IObjectContextAdapter).ObjectContext,
                SerializationType = typeof(Category)
            };
      
            var categories = dbContext.Categories.ToList();

            SetSerializer<Category>(serializationContext).SerializeAndDeserialize(categories);
        }

        [TestMethod]
        public void ISerializable()
        {
            dbContext.Configuration.ProxyCreationEnabled = false;

            var serializationContext = new CustomContext
            {
                ObjectContext = (dbContext as IObjectContextAdapter).ObjectContext,
                SerializationType = typeof(Product)
            };
          
            var products = dbContext.Products.ToList();
            SetSerializer<Product>(serializationContext).SerializeAndDeserialize(products);
        }

        [TestMethod]
        public void ISerializationSurrogate()
        {
            dbContext.Configuration.ProxyCreationEnabled = false;

            var serializationContext = new CustomContext
            {
                ObjectContext = (dbContext as IObjectContextAdapter).ObjectContext,
                SerializationType = typeof(Order_Detail)
            };

            var xmlSerializer = new NetDataContractSerializer()
            {
                SurrogateSelector = new OrderDetailSelector(),
                Context = new StreamingContext(StreamingContextStates.All, serializationContext)
            };

            var tester = GetTester<Order_Detail>(xmlSerializer);
           
            var orderDetails = dbContext.Order_Details.ToList();

            tester.SerializeAndDeserialize(orderDetails);
        }

        [TestMethod]
        public void IDataContractSurrogate()
        {
            dbContext.Configuration.ProxyCreationEnabled = true;
            dbContext.Configuration.LazyLoadingEnabled = true;

            var xmlSerializer = new DataContractSerializer(typeof(IEnumerable<Order>),
                new DataContractSerializerSettings
                {
                    DataContractSurrogate = new OrderDataContract()
                });
          
            var tester = GetTester<Order>(xmlSerializer);
            var orders = dbContext.Orders.ToList();

            tester.SerializeAndDeserialize(orders);
      }
       
    }   
}
