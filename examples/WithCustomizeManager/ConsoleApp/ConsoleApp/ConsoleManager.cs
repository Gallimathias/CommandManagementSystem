using CommandManagementSystem;
using CommandManagementSystem.Attributes;
using CommandManagementSystem.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    [CommandManager("ConsoleManager", "ConsoleApp.Commands")]
    internal class ConsoleManager : CommandManager<string, string, bool>
    {
        public ConsoleManager()
        {
            OnFinishedCommand += ConsoleManager_OnFinishedCommand;
        }

        private void ConsoleManager_OnFinishedCommand(ICommand<string, bool> command, string arg)
        {
            Console.WriteLine($"Command is Finish: {command.TAG}");
        }
    }
}
