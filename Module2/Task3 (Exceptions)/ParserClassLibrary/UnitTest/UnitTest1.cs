using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using ParserClassLibrary;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace UnitTest
{
    [TestClass]
    public class ParserTest
    {
        IntParser parser;

        public ParserTest()
        {
            SetUp();
        }
        [SetUp]
        public void SetUp()
        {
            parser = new IntParser();
        }
        [TestMethod]
        public void ParseStringToInt_ReturnInt()
        {          
            string s = "104";
         
            var actualResult = parser.Parse(s);

            Assert.AreEqual(actualResult, 104);
        }


        [TestMethod]
        public void ParseStringToInt_ReturnZero()
        {
            string s = "1MSD0";

            var actualResult = parser.Parse(s);

            Assert.AreEqual(actualResult, 0);
          
        //    Assert.ThrowsException<Exception>(() => parser.Parse(s));                      
        }


        [TestMethod]
        public void ParseEmptyStringToInt_ReturnZero()
        {
            string s = "";

            var actualResult = parser.Parse(s);

            Assert.AreEqual(actualResult, 0);                              
        }

    }
}
