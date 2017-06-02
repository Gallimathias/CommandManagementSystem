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
            manager = new ConsoleManager();

            foreach (var command in args)
            {
                if (!manager.Dispatch(command, "Sweet Home Sweet"))
                    throw new Exception("Command not succe");

            }
        }
    }
}
