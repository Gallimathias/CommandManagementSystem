using CommandManagementSystem.Attributes;
using CommandManagementSystem.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CommandManagementSystem
{
    /// <summary>
    /// An abstract standard implementation of a command
    /// </summary>
    /// <typeparam name="TParameter">The data type of the parameter to be passed</typeparam>
    /// <typeparam name="TOut">The data type of the dispatchresponse</typeparam>
    public abstract class Command<TParameter, TOut> : ICommand<TParameter, TOut>
    {
        /// <summary>
        /// The ExecutionOrder property contains all methods of the class registered
        /// for a dispatch in the correct order of their execution
        /// </summary>
        public static MethodInfo[] ExecutionOrder { get; protected set; }
        /// <summary>
        /// Returns a true if methods were registered for the dispatch in this class
        /// </summary>
        public static bool Registered => registered && ExecutionOrder != null && ExecutionOrder?.Length > 0;
        /// <summary>
        /// The number of executions by a dispatch
        /// </summary>
        protected int executionCount;

        private static bool registered;
        private static object tag;

        /// <summary>
        /// Contains the delegates for the next function to execute
        /// </summary>
        public Func<TParameter, TOut> NextFunction { get; protected set; }
        /// <summary>
        /// Returns whether the command has already gone through all steps
        /// </summary>
        public bool Finished { get; protected set; }
        /// <summary>
        /// Unique Indentifikator for the command
        /// </summary>
        public virtual object TAG => tag;

        /// <summary>
        /// Is thrown when the command has gone through all steps
        /// </summary>
        public virtual event FinishEventHandler<TParameter> FinishEvent;
        /// <summary>
        /// Is thrown when the command waits for the next dispatch
        /// </summary>
        public virtual event WaitEventHandler<TParameter, TOut> WaitEvent;

        /// <summary>
        /// An abstract standard implementation of a command
        /// </summary>
        public Command() => executionCount = 0;

        /// <summary>
        /// The main method is executed when dispatch if no Dispatch Order attribute found in the class.
        /// And set the NextFunction property to NULL.
        /// </summary>
        /// <param name="arg">The arguments passed by the dispatch</param>
        /// <returns>Returns the default value of the return type</returns>
        public virtual TOut Main(TParameter arg) => default(TOut);

        /// <summary>
        /// Executes the next action in the command
        /// </summary>
        /// <param name="arg">Parameters to be passed</param>
        /// <returns>Returns a fixed return value</returns>
        public virtual TOut Dispatch(TParameter arg)
        {
            if (Registered)
                NextFunction = (Func<TParameter, TOut>)ExecutionOrder[executionCount].
                    CreateDelegate(typeof(Func<TParameter, TOut>), this);
            else
                NextFunction = Main;

            executionCount++;
            var returnValue = NextFunction(arg);
            if (Registered)
            {
                if (executionCount < ExecutionOrder.Length)
                    RaiseWaitEvent(this, Dispatch);
                else
                    RaiseFinishEvent(this, arg);
            }
            else
            {
                if (NextFunction != null)
                    RaiseWaitEvent(this, Dispatch);
                else
                    RaiseFinishEvent(this, arg);
            }

            return returnValue;
        }

        /// <summary>
        /// Initializes the command
        /// </summary>
        /// <param name="arg">Parameters to be passed</param>
        /// <returns>Returns a fixed return value</returns>
        public virtual TOut Initialize(TParameter arg) => Dispatch(arg);

        /// <summary>
        /// Raises the finish event
        /// </summary>
        /// <param name="sender">This command</param>
        /// <param name="arg">The passed parameters</param>
        public virtual void RaiseFinishEvent(object sender, TParameter arg) => FinishEvent?.Invoke(sender, arg);

        /// <summary>
        /// Raises the wait event
        /// </summary>
        /// <param name="sender">This command</param>
        /// <param name="arg">The dispatch method</param>
        public virtual void RaiseWaitEvent(object sender, Func<TParameter, TOut> arg) => WaitEvent?.Invoke(sender, arg);

        /// <summary>
        /// Returns the tag of the command.
        /// Returns the current tag of the command by calling the ToString method of the command tag.
        /// </summary>
        /// <returns>Returns the tag of the command</returns>
        public override string ToString() => $"{TAG}";

        /// <summary>
        /// Registers the class in its static components. 
        /// Also registers all DispatchOrderAttribute-defined methods for Dispatch. 
        /// </summary>
        /// <param name="type">The type of this class</param>
        public static void Register(Type type)
        {
            tag = type?.GetCustomAttribute<CommandAttribute>()?.Tag;
            
            var actions = type?
                .GetMembers(
                    BindingFlags.NonPublic |
                    BindingFlags.Public |
                    BindingFlags.Instance |
                    BindingFlags.Static |
                    BindingFlags.FlattenHierarchy)?
                .Where(
                    m => m.GetCustomAttribute<DispatchOrderAttribute>() != null)?
                .ToArray();

            if (actions == null || actions?.Length == 0)
                return;

            var list = new List<KeyValuePair<DispatchOrderAttribute, MemberInfo>>();

            ExecutionOrder = new MethodInfo[actions.Length];

            for (int i = 0; i < actions.Length; i++)
            {
                var attr = actions[i].GetCustomAttribute<DispatchOrderAttribute>();
                list.Add(new KeyValuePair<DispatchOrderAttribute, MemberInfo>(attr, actions[i]));
            }

            list.OrderBy(b => b.Key.Order);

            for (int i = 0; i < list.Count; i++)
                ExecutionOrder[i] = (MethodInfo)list[i].Value;


            registered = true;
        }
    }

    /// <summary>
    /// An abstract standard implementation of a command with dynamic as return type
    /// </summary>
    /// <typeparam name="TParameter">The data type of the parameter to be passed</typeparam>
    public abstract class Command<TParameter> : Command<TParameter, dynamic> { }

    /// <summary>
    /// An abstract standard implementation of a command with dynamic as return type and 
    /// EventArgs As the parameter type 
    /// </summary>
    public abstract class Command : Command<object, dynamic> { }
}
