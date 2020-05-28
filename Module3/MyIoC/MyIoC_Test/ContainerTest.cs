using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyIoC;
using MyIoC.Test.InjectTestEntities;
using MyIoC_Test.InjectTestEntities;

namespace MyIoC_Test
{
    [TestClass]
    public class ContainerTest
    {
        Container container;

        [TestInitialize]
        public void Init()
        {
            container = new Container();
        }
     
        [TestMethod]
        public void CreateInstance_AssemblyAttributes_ConstructorInjectionTest()
        {
            container.AddAssembly(Assembly.GetExecutingAssembly());

            var customerBll = container.CreateInstance(typeof(CustomerBLL));
            var CustomerDal = (CustomerDAL)container.CreateInstance(typeof(ICustomerDAL));         

            Assert.IsTrue(customerBll.GetType() == typeof(CustomerBLL));
            Assert.IsTrue(CustomerDal.GetType() == typeof(CustomerDAL));            
        }    
        [TestMethod]
        public void CreateInstance_AssemblyAttributes_PropertiesInjectionTest()
        {
            container.AddAssembly(Assembly.GetExecutingAssembly());

            var customerBll = (CustomerBLL2)container.CreateInstance(typeof(CustomerBLL2));
       
            Assert.IsTrue(customerBll.GetType() == typeof(CustomerBLL2));       
            Assert.IsNotNull(customerBll.CustomerDAL.GetType() == typeof(CustomerDAL));         
            Assert.IsNotNull(customerBll.logger.GetType() == typeof(Logger));
        }
        [TestMethod]
        public void CreateInstance_ExplicitSet_ConstructorInjectionTest()
        {
            container.Register(typeof(CustomerBLL));
            container.Register(typeof(Logger));
            container.Register(typeof(ICustomerDAL), typeof(CustomerDAL));

            var customerBll = (CustomerBLL)container.CreateInstance(typeof(CustomerBLL));
            var CustomerDal = (CustomerDAL)container.CreateInstance(typeof(ICustomerDAL));

            Assert.IsTrue(customerBll.GetType() == typeof(CustomerBLL));
            Assert.IsTrue(CustomerDal.GetType() == typeof(CustomerDAL));
        }

        [TestMethod]
        public void CreateInstance_ExplicitSet_PropertiesInjectionTest()
        {
            container.Register(typeof(CustomerBLL2));
            container.Register(typeof(Logger));
            container.Register(typeof(ICustomerDAL), typeof(CustomerDAL));

            var customerBll = (CustomerBLL2)container.CreateInstance(typeof(CustomerBLL2));
           
            Assert.IsTrue(customerBll.GetType() == typeof(CustomerBLL2));          
            Assert.IsNotNull(customerBll.CustomerDAL.GetType() == typeof(CustomerDAL));          
            Assert.IsNotNull(customerBll.logger.GetType() == typeof(Logger));           
        }

        [TestMethod]
        public void GenericCreateInstance_ExplicitSet_ConstructorInjectionTest()
        {
            container.Register(typeof(CustomerBLL));
            container.Register(typeof(Logger));
            container.Register(typeof(ICustomerDAL), typeof(CustomerDAL));

            var customerBll = container.CreateInstance<CustomerBLL>();
        
            Assert.IsTrue(customerBll.GetType() == typeof(CustomerBLL));
        }
    }
}
