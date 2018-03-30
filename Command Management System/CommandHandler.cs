using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace CommandManagementSystem
{
    /// <summary>
    /// Manages individual commands as events
    /// </summary>
    /// <typeparam name="TIn">Data type of the command indentifier</typeparam>
    /// <typeparam name="TParameter">Data type of the command parameters</typeparam>
    /// <typeparam name="TOut">Return value of the dispatch</typeparam>
    public class CommandHandler<TIn, TParameter, TOut>
    {
        /// <summary>
        /// Commands waiting for a submit
        /// </summary>
        public ConcurrentQueue<KeyValuePair<TIn, TParameter>> CommandQueue { get; private set; }

        private CommandCollection<TIn, TParameter, TOut> mainCollection;

        /// <summary>
        /// Manages individual commands as events
        /// </summary>
        public CommandHandler()
        {
            mainCollection = new CommandCollection<TIn, TParameter, TOut>();
            CommandQueue = new ConcurrentQueue<KeyValuePair<TIn, TParameter>>();
        }

        /// <summary>
        /// Gives or sets a command to the specified identifier
        /// </summary>
        /// <param name="commandName">The command identifier</param>
        /// <returns>The command delegate</returns>
        public Func<TParameter, TOut> this[TIn commandName]
        {
            get
            {
                mainCollection.TryGetValue(commandName, out CommandHolder<TIn, TParameter, TOut> value);
                return value?.Delegate;
            }
            set => TryAdd(new CommandHolder<TIn, TParameter, TOut>(commandName, value));            
        }

        /// <summary>
        /// Dispatched the command with the specified identifier and passed the parameters
        /// </summary>
        /// <param name="commandName">The command identifier</param>
        /// <param name="parameter">The parameters to be transferred</param>
        /// <returns>Returns the set value</returns>
        public TOut Dispatch(TIn commandName, TParameter parameter) => mainCollection[commandName].Delegate(parameter);

        /// <summary>
        /// Does not dispose of a command until the submit method is called
        /// </summary>
        /// <param name="commandName">The command identifier</param>
        /// <param name="parameter">The parameters to be transferred</param>
        public void DispatchOnSubmit(TIn commandName, TParameter parameter) => CommandQueue.Enqueue(new KeyValuePair<TIn, TParameter>(commandName, parameter));

        /// <summary>
        /// Dispatch all commands in que
        /// </summary>
        /// <returns>Returns the set value</returns>
        public TOut Submit()
        {
            TOut returnValue = default(TOut);

            while (!CommandQueue.IsEmpty)
            {
                if (CommandQueue.TryDequeue(out KeyValuePair<TIn, TParameter> command))
                    returnValue = Dispatch(command.Key, command.Value);
            }

            return returnValue;
        }

        /// <summary>
        /// Checks whether a command with the specified identifier is already registered
        /// </summary>
        /// <param name="commandName">The command identifier</param>
        /// <returns>Returns a true if the command is already registered</returns>
        public bool CommandExists(TIn commandName) => mainCollection.ContainsKey(commandName);

        public bool TryAdd(CommandHolder<TIn, TParameter, TOut> commandHolder)
        {
            var returnValue = mainCollection.TryAdd(commandHolder);
            if (!returnValue)
               return mainCollection.TryUpdate(commandHolder);

            return returnValue;
        }

        public bool TryUpdate(TIn tag, Func<TParameter, TOut> action) => mainCollection.TryUpdate(tag, action);

        public List<KeyValuePair<TIn, TIn[]>> GetTagList() => mainCollection.GetTagList().ToList();

    }

    /// <summary>
    /// Manages individual commands as events with string as command indentifier type
    /// </summary>
    /// <typeparam name="TParameter">Data type of the command parameters</typeparam>
    /// <typeparam name="TOut">Return value of the dispatch</typeparam>
    public class CommandHandler<TParameter, TOut> : CommandHandler<string, TParameter, TOut> { }

    /// <summary>
    /// Manages individual commands as events with string as command indentifier type and
    /// dynamic as return value
    /// </summary>
    /// <typeparam name="TParameter">Data type of the command parameters</typeparam>
    public class CommandHandler<TParameter> : CommandHandler<TParameter, dynamic> { }

    /// <summary>
    /// Manages individual commands as events with string as command indentifier type and
    /// dynamic as return value and EventArgs as parameter type
    /// </summary>
    public class CommandHandler : CommandHandler<object> { }
}
