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

        [Command("Default")]
        public static int DefaultCommand(string args)
        {
            return 14;
        }
    }

    [StringCommand]
    public class StringComplex : Command<string, int>
    {
        public StringComplex()
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

        
        [StringCommand]
        public static int DefaultString(string args)
        {
            return 14;
        }
    }

}
