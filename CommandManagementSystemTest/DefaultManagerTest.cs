using CommandManagementSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandManagementSystemTest
{
    [TestClass]
    public class DefaultManagerTest
    {
        [TestMethod]
        public void DefaultManagerInstance()
        {
            var manager = new DefaultCommandManager<string, int>("CommandManagementSystemTest");

            for (int i = 0; i < 10; i++)
            {
                Assert.AreEqual(1, manager.Dispatch("test", ""));
                Assert.AreEqual(2, manager.Dispatch("test", ""));
                Assert.AreEqual(1, manager.Dispatch("t", ""));
                Assert.AreEqual(2, manager.Dispatch("t", ""));
            }
        }
    }
}
