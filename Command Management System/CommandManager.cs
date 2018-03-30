using CommandManagementSystem.Attributes;
using CommandManagementSystem.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CommandManagementSystem
{
    /// <summary>
    /// An abstract base implementation of a command manager
    /// </summary>
    /// <typeparam name="TIn">Data type of command indentifiers</typeparam>
    /// <typeparam name="TParameter">Data type of command parameters</typeparam>
    /// <typeparam name="TOut">Return value of the dispatch method</typeparam>
    public abstract class CommandManager<TIn, TParameter, TOut> : ICommandManager<TIn, TParameter, TOut>
    {
        /// <summary>
        /// The main command handler
        /// </summary>
        protected CommandHandler<TIn, TParameter, TOut> commandHandler;
        /// <summary>
        /// Dictionary with waiting commands
        /// </summary>
        protected ConcurrentDictionary<TIn, Func<TParameter, TOut>> waitingDictionary;
        protected ConcurrentDictionary<TIn, TIn[]> aliasDictionary;

        /// <summary>
        /// Delegate for command manager events
        /// </summary>
        /// <param name="command">Triggering command</param>
        /// <param name="arg">Passed parameters for the command</param>
        public delegate void CommandManagerEventHandler(ICommand<TParameter, TOut> command, TParameter arg);
        /// <summary>
        /// Dispatched when a command is completed
        /// </summary>
        public virtual event CommandManagerEventHandler OnFinishedCommand;
        /// <summary>
        /// Dispatched when a command is waiting
        /// </summary>
        public virtual event CommandManagerEventHandler OnWaitingCommand;

        /// <summary>
        /// An abstract base implementation of a command manager. With control over the initialization
        /// </summary>
        /// <param name="initialize">If this value is set to false, no commands are searched by the manager.</param>
        public CommandManager(bool initialize)
        {
            commandHandler = new CommandHandler<TIn, TParameter, TOut>();
            aliasDictionary = new ConcurrentDictionary<TIn, TIn[]>();
            waitingDictionary = new ConcurrentDictionary<TIn, Func<TParameter, TOut>>();

            if (initialize)
                Initialize();
        }
        /// <summary>
        /// An abstract base implementation of a command manager
        /// </summary>        
        public CommandManager() : this(true)
        {

        }

        /// <summary>
        /// Initializes the command manager and registers the 
        /// corresponding commands at the command handler
        /// </summary>
        public virtual void Initialize()
        {
            var commandNamespace = GetType().GetCustomAttribute<CommandManagerAttribute>()?.CommandNamespaces;
            var types = Assembly.GetAssembly(GetType()).GetTypes();

            if (commandNamespace == null)
                commandNamespace = new[] { GetType().Namespace };

            var commands = types.Where(
                t => t.GetCustomAttribute<CommandAttribute>() != null && commandNamespace.Contains(t.Namespace)).ToList();

            foreach (var command in commands)
            {
                command.GetMethod(
                    "Register",
                    BindingFlags.Static |
                    BindingFlags.Public |
                    BindingFlags.FlattenHierarchy)
                    .Invoke(null, new[] { command });
                //commandHandler[(TIn)command.GetCustomAttribute<CommandAttribute>().Tag] = (e)
                //    => InitializeCommand(command, e);

                var commandAttribute = command.GetCustomAttribute<CommandAttribute>();
                var holder = new CommandHolder<TIn, TParameter, TOut>((TIn)commandAttribute.Tag)
                {
                    Delegate = (e) => InitializeCommand(command, e),
                    Aliases = commandAttribute.Aliases.Select(a => (TIn)a).ToArray()
                };
                commandHandler.TryAdd(holder);
                aliasDictionary.TryAdd(holder.Tag, holder.Aliases);
            }

            InitializeOneTimeCommand(commandNamespace, types);
        }

        /// <summary>
        /// Dispatch the specified command and pass the parameters
        /// </summary>
        /// <param name="command">The command Indentifier</param>
        /// <param name="arg">The parameters to be transferred</param>
        /// <returns>Returns the result of the dispatch</returns>
        public virtual TOut Dispatch(TIn command, TParameter arg)
        {
            if (!aliasDictionary.ContainsKey(command))
                command = aliasDictionary.FirstOrDefault(t => t.Value.Contains(command)).Key;

            if (waitingDictionary.ContainsKey(command))
            {
                if (!waitingDictionary.TryGetValue(command, out Func<TParameter, TOut> method))
                    throw new Exception($"Dispatch failed to retrieve the Waiting {command} command");
                else
                    return method(arg);
            }
            else
            {
                return commandHandler.Dispatch(command, arg);
            }
        }

        /// <summary>
        /// Dispatch the specified command and pass the parameters asynchronous
        /// </summary>
        /// <param name="command">The command Indentifier</param>
        /// <param name="arg">The parameters to be transferred</param>
        /// <returns>Returns the result of the dispatch</returns>
        public virtual Task<TOut> DispatchAsync(TIn command, TParameter arg) =>
            Task.Run(() => Dispatch(command, arg));

        /// <summary>
        /// Initializes the passed command with the parameters
        /// </summary>
        /// <param name="command">The command</param>
        /// <param name="arg">The parameters to be transferred</param>
        /// <returns>Returns the result of the initialize</returns>
        public virtual TOut InitializeCommand(ICommand<TParameter, TOut> command, TParameter arg)
        {
            command.FinishEvent += CommandFinishEvent;
            command.WaitEvent += CommandWaitEvent;

            return command.Initialize(arg);
        }
        /// <summary>
        /// Initializes a command from the specified command datatype with the parameters
        /// </summary>
        /// <param name="commandType">The command data type</param>
        /// <param name="arg">The parameters to be transferred</param>
        /// <returns>Returns the result of the initialize</returns>
        public virtual TOut InitializeCommand(Type commandType, TParameter arg) =>
            InitializeCommand((ICommand<TParameter, TOut>)Activator.CreateInstance(commandType), arg);
        /// <summary>
        /// Initializes a command from the specified command datatype with
        /// the parameters and start parameters for the constructor
        /// </summary>
        /// <param name="commandType">The command data type</param>
        /// <param name="arg">The parameters to be transferred</param>
        /// <param name="startParams">Parameters passed to the constructor</param>
        /// <returns>Returns the result of the initialize</returns>
        public virtual TOut InitializeCommand(Type commandType, TParameter arg, params object[] startParams) =>
            InitializeCommand((ICommand<TParameter, TOut>)Activator.CreateInstance(commandType, startParams), arg);

        public List<KeyValuePair<TIn, TIn[]>> GetCommandTagList() => commandHandler.GetTagList();

        /// <summary>
        /// Executed when a command is finished
        /// </summary>
        /// <param name="sender">The triggering command</param>
        /// <param name="arg">The command parameters</param>
        protected virtual void CommandFinishEvent(object sender, TParameter arg)
        {
            var command = (ICommand<TParameter, TOut>)sender;

            waitingDictionary.TryRemove((TIn)command.Tag, out Func<TParameter, TOut> method);

            if (command.Reinitialize)
            {
                commandHandler.TryUpdate((TIn)command.Tag, (e) => InitializeCommand(command.GetType(), e));
                //commandHandler[(TIn)command.Tag] = (e) => InitializeCommand(command.GetType(), e);
            }

            OnFinishedCommand?.Invoke(command, arg);
        }
        /// <summary>
        /// This method is obsolete please use instead CommandFinishEvent.
        /// </summary>
        /// <param name="sender">The triggering command</param>
        /// <param name="arg">The command parameters</param>
        [Obsolete("This method has been renamed to CommandFinishEvent")]
        public virtual void Command_FinishEvent(object sender, TParameter arg) => CommandFinishEvent(sender, arg);

        /// <summary>
        /// Executed when a command is waiting
        /// </summary>
        /// <param name="sender">The triggering command</param>
        /// <param name="arg">The dispatch method</param>
        protected virtual void CommandWaitEvent(object sender, Func<TParameter, TOut> arg)
        {
            if (arg == null && sender == null)
                return;

            var command = (ICommand<TParameter, TOut>)sender;

            if (!waitingDictionary.TryAdd((TIn)command.Tag, arg))
                waitingDictionary.TryUpdate((TIn)command.Tag, arg, arg);
        }
        /// <summary>
        /// This method is obsolete please use instead CommandWaitEvent.
        /// </summary>
        /// <param name="sender">The triggering command</param>
        /// <param name="arg">The dispatch method</param>
        [Obsolete("This method has been renamed to CommandWaitEvent")]
        public virtual void Command_WaitEvent(object sender, Func<TParameter, TOut> arg) => CommandWaitEvent(sender, arg);

        /// <summary>
        /// Searches and registers all methods in the given namespace in the TypeArray,
        /// which have a OneTimeCommandAttribute as a Command.
        /// </summary>
        /// <param name="namespaces">Namespaces in which the method searches</param>
        /// <param name="types">TypeArray that searches the method</param>
        protected void InitializeOneTimeCommand(string[] namespaces, Type[] types)
        {
            var commandClasses = types.Where(
                t => namespaces.Contains(t.Namespace)).ToArray();

            foreach (var commandClass in commandClasses)
            {
                var members = commandClass
                    .GetRuntimeMethods()
                    //.GetMembers(
                    //    BindingFlags.NonPublic |
                    //    BindingFlags.Public |
                    //    BindingFlags.Instance |
                    //    BindingFlags.FlattenHierarchy |
                    //    BindingFlags.Static)
                    .Where(
                        m => m.GetCustomAttribute<CommandAttribute>() != null);

                foreach (var member in members)
                {
                    var commandAttribute = member.GetCustomAttribute<CommandAttribute>();
                    var commandHolder = new CommandHolder<TIn, TParameter, TOut>((TIn)commandAttribute.Tag)
                    {
                        Aliases = commandAttribute.Aliases.Select(a => (TIn)a).ToArray(),
                        Delegate = (Func<TParameter, TOut>)member.CreateDelegate(typeof(Func<TParameter, TOut>))
                    };
                    commandHandler.TryAdd(commandHolder);
                    aliasDictionary.TryAdd(commandHolder.Tag, commandHolder.Aliases);
                    //commandHandler[(TIn)member.GetCustomAttribute<CommandAttribute>().Tag] = (Func<TParameter, TOut>)
                    //    member.CreateDelegate(typeof(Func<TParameter, TOut>));
                }

            }
        }
        /// <summary>
        /// Searches and registers all methods that have a OneTimeCommandAttribute as a command in the 
        /// specified namespace in the assembly where the CommandManager is defined.
        /// </summary>
        /// <param name="namespaces">Namespaces in which the method searches</param>
        protected void InitializeOneTimeCommand(string[] namespaces) =>
            InitializeOneTimeCommand(namespaces, Assembly.GetAssembly(GetType()).GetTypes());
        /// <summary>
        /// Find and register all methods that has a OneTimeCommandAttribute as a command in the namespaces registered
        /// with the manager in the assembly where the CommandManager is defined.
        /// </summary>
        protected void InitializeOneTimeCommand() =>
            InitializeOneTimeCommand(GetType().GetCustomAttribute<CommandManagerAttribute>().CommandNamespaces);
    }

    /// <summary>
    /// An abstract base implementation of a command manager with string as command indentifiers
    /// </summary>
    /// <typeparam name="TParameter">Data type of command parameters</typeparam>
    /// <typeparam name="TOut">Return value of the dispatch method</typeparam>
    public abstract class CommandManager<TParameter, TOut> : CommandManager<string, TParameter, TOut> { }

    /// <summary>
    /// An abstract base implementation of a command manager with string as command indentifiers
    /// and dynamic as result data type
    /// </summary>
    /// <typeparam name="TParameter">Data type of command parameters</typeparam>
    public abstract class CommandManager<TParameter> : CommandManager<TParameter, dynamic> { }

    /// <summary>
    /// An abstract base implementation of a command manager with string as command indentifiers
    /// and dynamic as result data type and EventArgs as parameter type
    /// </summary>
    public abstract class CommandManager : CommandManager<object> { }
}
