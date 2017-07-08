using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static ConsoleManager manager;

        static void Main(string[] args)
        {
            manager = new ConsoleManager(); //Instance a new custom CommandManager

            foreach (var command in args)
            {
                if (!manager.Dispatch(command, "Sweet Home Sweet")) //Dispatch the commands from args with parameter
                    throw new Exception("Command not succes"); //Throw if the Dispatch returns false
            }

            Console.WriteLine("Please press any key"); //After all Dispatchs
            Console.ReadKey();
        }
    }
}
