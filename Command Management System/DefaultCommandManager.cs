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
            Namespaces = new List<string>();
            Initialize(Assembly.GetCallingAssembly(), namespaces);
        }

        public new void Initialize(Assembly assembly, params string[] namespaces)
        {
            if (Namespaces == null)
                Namespaces = new List<string>();

            Namespaces.AddRange(namespaces);

            var commands = assembly.GetTypes().Where(
               t => t.GetCustomAttribute<CommandAttribute>() != null && Namespaces.Contains(t.Namespace)).ToList();

            foreach (var command in commands)
            {
                command.GetMethod(
                   "Register",
                   BindingFlags.Static |
                   BindingFlags.Public |
                   BindingFlags.FlattenHierarchy)
                   .Invoke(null, new[] { command });
                commandHandler[(string)command.GetCustomAttribute<CommandAttribute>().Tag] += (e)
                    => InitializeCommand(command, e);
            }

            InitializeOneTimeCommand(Namespaces.ToArray(), assembly.GetTypes());
            IsInitialized = true;
        }
        public new void Initialize(params string[] namespaces) => Initialize(Assembly.GetCallingAssembly(), namespaces);

        public override void Initialize()
        {
            return;
        }
    }
}
