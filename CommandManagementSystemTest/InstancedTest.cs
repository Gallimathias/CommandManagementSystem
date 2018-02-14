using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommandManagementSystemTest
{
    [TestClass]
    public class InstancedTest
    {
        [TestMethod]
        public void Main()
        {
            var manager = new Manager();

            for (int i = 0; i < 10; i++)
            {
                Assert.AreEqual(1, manager.Dispatch("test", ""));
                Assert.AreEqual(2, manager.Dispatch("test", ""));
            }

        }
    }
}
