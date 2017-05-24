using CommandManagementSystem.Attributes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommandManagementSystem
{
    public class DefaultCommandManager : CommandManager<string, EventArgs, object>
    {
        public List<string> Namespaces { get; private set; }

        public bool IsInitialized { get; private set; }

        public DefaultCommandManager(params string[] namespaces) 
        {
            commandHandler = new CommandHandler<string, EventArgs, object>();
            waitingDictionary = new ConcurrentDictionary<string, Func<EventArgs, object>>();
            Namespaces = new List<string>();
            Initialize(namespaces);
        }

        public new void Initialize(params string[] namespaces)
        {
            if (Namespaces == null)
                Namespaces = new List<string>();

            Namespaces.AddRange(namespaces);

            var commands = Assembly.GetExecutingAssembly().GetTypes().Where(
               t => t.GetCustomAttribute<CommandAttribute>() != null && Namespaces.Contains(t.Namespace)).ToList();

            foreach (var command in commands)
            {
                command.GetMethod("Registration").Invoke(null, null);
                commandHandler[(string)command.GetCustomAttribute<CommandAttribute>().Tag] += (e)
                    => InitializeCommand(command, e);
            }

            InitializeOneTimeCommand(Namespaces.ToArray());
            IsInitialized = true;
        }

    }
}
