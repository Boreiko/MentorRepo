using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using FileSystemVisitorApp;
using System.IO;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using Action = FileSystemVisitorApp.Action;

namespace UnitTestFSV
{
    [TestClass]
    public class FileSystemVisitorTest
    {
        Mock<FileSystemInfo> fsiMoq;
        FileSystemVisitor fsv;
        public FileSystemVisitorTest() { TestInit(); }

        [SetUp]
        public void TestInit()
        {
               fsv = new FileSystemVisitor();
            fsiMoq = new Mock<FileSystemInfo>();
        }

        [TestMethod]
        public void Item_SkipElement()
        {           
            FileSystemInfo fileSystemInfo = fsiMoq.Object;
            
            var action = fsv.ProcessItem(fileSystemInfo, (obj, args) => { fsv.action = Action.Skip; }, null);
           
            Assert.AreEqual(action, Action.Skip);
        }

        [TestMethod]
        public void FilteredItem_SkipElement()
        {
            FileSystemInfo fileSystemInfo = fsiMoq.Object;
            fsv.filter = filt => true;
            
            var action = fsv.ProcessItem(fileSystemInfo, (obj, args) => { fsv.action = Action.Skip; }, (obj, args) => { });
            
            Assert.AreEqual(action, Action.Skip);
        }

        [TestMethod]
        public void Item_StopElement()
        {
            FileSystemInfo fileSystemInfo = fsiMoq.Object;

            var action = fsv.ProcessItem(fileSystemInfo, (obj, args) => { fsv.action = Action.Stop; }, null);
            
            Assert.AreEqual(action, Action.Stop);
        }

        [TestMethod]
        public void FilteredItem_StopElement()
        {
            FileSystemInfo fileSystemInfo = fsiMoq.Object;
            fsv.filter = filt => true;
            
            var action = fsv.ProcessItem(fileSystemInfo, (obj, args) => { fsv.action = Action.Stop; }, (obj, args) => { });
            
            Assert.AreEqual(action, Action.Stop);
        }

        [TestMethod]
        public void Item_ContinueAction()
        {
            FileSystemInfo fileSystemInfo = fsiMoq.Object;

            var action = fsv.ProcessItem(fileSystemInfo, (obj, args) => { fsv.action = Action.Continue; }, null);
            
            Assert.AreEqual(action, Action.Continue);
        }

        [TestMethod]
        public void FilteredItem_ContinueAction()
        {
            FileSystemInfo fileSystemInfo = fsiMoq.Object;
            fsv.filter = filt => true;
            
            var action = fsv.ProcessItem(fileSystemInfo, (obj, args) => { fsv.action = Action.Continue; }, (obj, args) => { });
            
            Assert.AreEqual(action, Action.Continue);
        }

    }
}
