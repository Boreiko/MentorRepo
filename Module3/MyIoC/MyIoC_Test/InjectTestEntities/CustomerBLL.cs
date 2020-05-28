using MyIoC.Attributes;
using MyIoC_Test.InjectTestEntities;
using System.ComponentModel.Composition;

namespace MyIoC.Test.InjectTestEntities
{
    [ImportConstructor]
    public class CustomerBLL
    {
        public CustomerBLL(ICustomerDAL dal, Logger logger)
        { }
        public CustomerBLL() { }
    }

    public class CustomerBLL2
    {
        [Attributes.Import]
        public ICustomerDAL CustomerDAL { get; set; }
        [Attributes.Import]
        public Logger logger { get; set; }

    }
}
