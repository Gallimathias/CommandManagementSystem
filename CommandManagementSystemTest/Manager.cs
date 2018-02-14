using CommandManagementSystem;
using CommandManagementSystem.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandManagementSystemTest
{
    [CommandManager("Manager", "CommandManagementSystemTest")]
    class Manager : CommandManager<string, string, int>
    {
    }
}
