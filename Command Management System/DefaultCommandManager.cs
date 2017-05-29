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
    [CommandManager("DefaultCommandManager")]
    public class DefaultCommandManager : CommandManager<string, object[], dynamic>
    {
        public List<string> Namespaces { get; private set; }

        public bool IsInitialized { get; private set; }

        public DefaultCommandManager(params string[] namespaces)
        {
            commandHandler = new CommandHandler<string, object[], dynamic>();
            waitingDictionary = new ConcurrentDictionary<string, Func<object[], dynamic>>();
            Namespaces = new List<string>();
            Initialize(namespaces);
        }

        public new void Initialize(params string[] namespaces)
        {
            if (Namespaces == null)
                Namespaces = new List<string>();

            Namespaces.AddRange(namespaces);

            var commands = Assembly.GetAssembly(GetType()).GetTypes().Where(
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

        public override void Initialize()
        {
            return;
            //base.Initialize();
        }
    }
}
