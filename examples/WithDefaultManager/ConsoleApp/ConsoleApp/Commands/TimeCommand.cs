using CommandManagementSystem;
using CommandManagementSystem.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Commands
{
    [Command("/time")]
    public class TimeCommand : Command<object[]>
    {
        public override dynamic Main(object[] arg)
        {
            Console.WriteLine(DateTime.Now);
            return base.Main(arg);
        }
    }
}
