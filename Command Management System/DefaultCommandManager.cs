using CommandManagementSystem.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommandManagementSystem
{
    public abstract class DefaultCommandManager : CommandManager<string, EventArgs, object>
    {
        public static List<string> Namespaces { get; private set; }

        private static DefaultCommandManager defaultCommandManager;

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

            base.InitializeOneTimeCommand(Namespaces.ToArray());
        }

    }
}
