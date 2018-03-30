using CommandManagementSystem;
using CommandManagementSystem.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandManagementSystemTest
{
    [Command("test", "t")]
    public class ComplexCommand : Command<string, int>
    {
        public ComplexCommand()
        {

        }

        [DispatchOrder(0)]
        public int ExecuteFirst(string args)
        {
            return 1;
        }

        [DispatchOrder(1)]
        public int ExecuteSecond(string args)
        {
            return 2;
        }
    }
}
