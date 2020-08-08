using Microsoft.VisualStudio.TestTools.UnitTesting;
using AxelotTestAPI.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Sdk;
using System.ComponentModel;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;
using AxelotTestAPI.Extentions;

namespace AxelotTestAPI.Domain.Exceptions.Tests
{
    enum TestCase
    {
        [Description("Один")]
        one=0,
        two=1
    }

    [TestClass()]
    public class EnumFriendlyNamesTests
    {


        [TestMethod()]
        public void GetDescriptionTest()
        {
            var k = TestCase.one;
            Assert.AreEqual("Один", k.GetDescription());
            k = TestCase.two;
            Assert.AreEqual("two", k.GetDescription());
        }
    }
}