using CommandManagementSystem.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Commands
{
    static class OneTimeCommands
    {
        [OneTimeCommand("/hello")]
        public static bool Hello(string arg)
        {
            Console.WriteLine("Hello");
            return true;
        }
    }
}
