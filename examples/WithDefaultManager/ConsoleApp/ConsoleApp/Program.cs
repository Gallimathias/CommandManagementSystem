using CommandManagementSystem;
using CommandManagementSystem.Attributes;
using ConsoleApp.Commands;
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
            defaultCommandManager = new DefaultCommandManager("ConsoleApp.Commands", "ConsoleApp"); //New instance of a standard manager with two search areas

            foreach (var command in args)
                defaultCommandManager.Dispatch(command, null); //Dispatch a new command from args

            Console.ReadKey();
        }

        [Command("/hello")] //OneTime command without creating an object
        static dynamic Hello(object[] arg)
        {
            Console.WriteLine("Hello");
            return null;
        }
    }
}
