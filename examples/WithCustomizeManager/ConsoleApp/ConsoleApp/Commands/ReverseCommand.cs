using CommandManagementSystem;
using CommandManagementSystem.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Commands
{
    [Command("/reverse", "/r")] //Complex command with several versions and stages
    public class ReverseCommand : Command<string, bool>
    {
        Random rand;

        public ReverseCommand()
        {
            rand = new Random();
        }

        [DispatchOrder(0)] //Is executed the first time
        public bool GetChaos(string arg)
        {
            if (arg.Length < 10)
                return false;

            Console.Write("Random: ");

            for (int i = 0; i < arg.Length; i++)
            {
                var c = rand.Next(arg.Length);
                Console.Write(arg[c]);
            }

            Console.WriteLine();
            return true;
        }

        [DispatchOrder(1)] //Runs the second time
        public bool GetRevert(string arg)
        {
            if (arg.Length < 10)
                return false;

            Console.Write("Reverse: ");

            for (int i = arg.Length - 1; i >= 0; i--)
            {
                Console.Write(arg[i]);
            }

            Console.WriteLine();
            return true;
        }
    }
}
