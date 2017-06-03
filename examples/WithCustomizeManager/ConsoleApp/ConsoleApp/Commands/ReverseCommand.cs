using CommandManagementSystem;
using CommandManagementSystem.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Commands
{
    [Command("/reverse")]
    public class ReverseCommand : Command<string, bool>
    {
        Random rand;

        public ReverseCommand()
        {
            rand = new Random();
        }

        [DispatchOrder(0)]
        public bool GetChaos(string arg)
        {
            if (arg.Length < 10)
                return false;

            for (int i = 0; i < arg.Length; i++)
            {
                var c = rand.Next(arg.Length);
                Console.Write(arg[c]);
            }

            return true;
        }

        [DispatchOrder(1)]
        public bool GetRevert(string arg)
        {
            if (arg.Length < 10)
                return false;

            for (int i = arg.Length; i > 0; i--)
            {
                Console.Write(arg[i]);
            }

            return true;
        }
    }
}
