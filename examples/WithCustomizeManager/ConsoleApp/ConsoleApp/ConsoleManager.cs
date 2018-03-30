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
    /// <summary>
    /// This is a custom CommandManager
    /// </summary>
    [CommandManager("ConsoleManager", "ConsoleApp.Commands")] //Mark this CommandManager with a tag and search scope
    internal class ConsoleManager : CommandManager<string, string, bool> //Inherited from the CommandManager class with string as command, string as parameter, and bool as return value
    {
        public ConsoleManager()
        {
            OnFinishedCommand += ConsoleManager_OnFinishedCommand; //Subscribe to the event to see when a command is ready
        }

        private void ConsoleManager_OnFinishedCommand(ICommand<string, bool> command, string arg)
        {
            Console.WriteLine($"Command is Finish: {command.Tag}");
        }
    }
}
