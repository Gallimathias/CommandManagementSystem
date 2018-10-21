using CommandManagementSystem.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CommandManagementSystem
{
    [CommandManager("DefaultCommandManager")]
    public class DefaultCommandManager<TIn, TParameter, TOut> : CommandManager<TIn, TParameter, TOut>
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
        public DefaultCommandManager(params string[] namespaces)
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
               t => t.GetCustomAttribute<CommandAttribute>() != null && Namespaces.Contains(t.Namespace)).ToList();

            foreach (var command in commands)
            {
                command.GetMethod(
                   "Register",
                   BindingFlags.Static |
                   BindingFlags.Public |
                   BindingFlags.FlattenHierarchy)
                   .Invoke(null, new[] { command });

                var commandAttribute = command.GetCustomAttribute<CommandAttribute>();
                var commandHolder = new CommandHolder<TIn, TParameter, TOut>((TIn)commandAttribute.Tag)
                {
                    Aliases = commandAttribute.Aliases.Select(a => (TIn)a).ToArray(),
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

        /// <summary>
        /// The overwritten default initialization method. 
        /// This method contains only one return and does not call base. Otherwise, errors occur
        /// </summary>
        public override void Initialize() { }
    }

    [CommandManager("DefaultCommandManager")]
    public class DefaultCommandManager<TParameter, TOut> : DefaultCommandManager<string, TParameter, TOut>
    {
        public DefaultCommandManager(params string[] namespaces) : base(namespaces) { }
    }

    [CommandManager("DefaultCommandManager")]
    public class DefaultCommandManager<TParameter> : DefaultCommandManager<TParameter, object>
    {
        public DefaultCommandManager(params string[] namespaces) : base(namespaces) { }
    }

    [CommandManager("DefaultCommandManager")]
    public class DefaultCommandManager : DefaultCommandManager<object>
    {
        public DefaultCommandManager(params string[] namespaces) : base(namespaces) { }
    }
}
