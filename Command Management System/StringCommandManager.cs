using CommandManagementSystem.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CommandManagementSystem
{
    [CommandManager(nameof(StringCommandManager))]
    public class StringCommandManager<TParameter, TOut> : CommandManager<string, TParameter, TOut>
    {
        /// <summary>
        /// Namespaces that were registered in the default Command Manager
        /// </summary>
        public List<string> Namespaces { get; private set; }

        /// <summary>
        /// This property returns true if the manager has been initialized
        /// </summary>
        public bool IsInitialized { get; private set; }

        /// <summary>
        /// A standard implementation of a Command Manager. 
        /// </summary>
        /// <param name="namespaces">Namespace to be registered in the manager</param>
        public StringCommandManager(params string[] namespaces)
        {
            Namespaces = new List<string>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                Initialize(assembly, namespaces);
        }

        /// <summary>
        /// Initalizes the manager with the specified namespaces in the specified assembly
        /// </summary>
        /// <param name="assembly">Assembly in which is searched</param>
        /// <param name="namespaces">Namespace to be registered in the manager</param>
        public void Initialize(Assembly assembly, params string[] namespaces)
        {
            if (Namespaces == null)
                Namespaces = new List<string>();

            Namespaces.AddRange(namespaces);

            var commands = assembly.GetTypes().Where(
               t => t.GetCustomAttribute<StringCommandAttribute>() != null && Namespaces.Contains(t.Namespace)).ToList();

            foreach (var command in commands)
            {
                command.GetMethod(
                   "Register",
                   BindingFlags.Static |
                   BindingFlags.Public |
                   BindingFlags.FlattenHierarchy)
                   .Invoke(null, new[] { command });

                var commandAttribute = command.GetCustomAttribute<CommandAttribute>();
                var commandHolder = new CommandHolder<string, TParameter, TOut>(command.Name)
                {
                    Aliases = commandAttribute.Aliases.Select(a => (string)a).ToArray(),
                    Delegate = (e) => InitializeCommand(command, e)
                };
                commandHandler.TryAdd(commandHolder);
                aliasDictionary.TryAdd(commandHolder.Tag, commandHolder.Aliases);
            }

            InitializeOneTimeCommand(Namespaces.ToArray(), assembly.GetTypes());
            IsInitialized = true;
        }
        public void Initialize(params string[] namespaces)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                Initialize(assembly, namespaces);
        }

        protected override void InitializeOneTimeCommand(string[] namespaces, Type[] types)
        {
            var commandClasses = types.Where(
               t => namespaces.Contains(t.Namespace)).ToArray();

            foreach (var commandClass in commandClasses)
            {
                var members = commandClass
                    .GetRuntimeMethods()
                    .Where(
                        m => m.GetCustomAttribute<StringCommandAttribute>() != null);

                foreach (var member in members)
                {
                    var commandAttribute = member.GetCustomAttribute<CommandAttribute>();
                    var commandHolder = new CommandHolder<string, TParameter, TOut>(member.Name)
                    {
                        Aliases = commandAttribute.Aliases.Select(a => (string)a).ToArray(),
                        Delegate = (Func<TParameter, TOut>)member.CreateDelegate(typeof(Func<TParameter, TOut>))
                    };
                    commandHandler.TryAdd(commandHolder);
                    aliasDictionary.TryAdd(commandHolder.Tag, commandHolder.Aliases);
                }

            }
        }

        /// <summary>
        /// The overwritten default initialization method. 
        /// This method contains only one return and does not call base. Otherwise, errors occur
        /// </summary>
        public override void Initialize() { }
    }

    [CommandManager(nameof(StringCommandManager))]
    public class StringCommandManager<TOut> : StringCommandManager<string[], TOut>
    {

    }

    [CommandManager(nameof(StringCommandManager))]
    public sealed class StringCommandManager : StringCommandManager<string[], object>
    {

    }
}
