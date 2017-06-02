using CommandManagementSystem;
using CommandManagementSystem.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static DefaultCommandManager defaultCommandManager;

        static void Main(string[] args)
        {
            defaultCommandManager = new DefaultCommandManager("ConsoleApp.Commands", "ConsoleApp");

            foreach (var command in args)
                defaultCommandManager.Dispatch(command, null);

            Console.ReadKey();
        }

        [OneTimeCommand("/hello")]
        static dynamic Hello(object[] arg)
        {
            Console.WriteLine("Hello");
            return null;
        } 
    }
}
