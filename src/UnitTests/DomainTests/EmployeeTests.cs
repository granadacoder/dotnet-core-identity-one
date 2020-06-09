
using GranadaCoder.IdentityDemo.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GranadaCoder.IdentityDemo.UnitTests.DomainTests
{
    [TestClass]
    public class EmployeeTests
    {
        [TestMethod]
        public void EmployeeScalarsTest()
        {
            Employee testItem = new Employee();
            testItem.FirstName = "abc";
            Assert.AreEqual("abc", testItem.FirstName);
        }
    }
}
