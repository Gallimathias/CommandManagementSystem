using CommandManagementSystem.Attributes;
using CommandManagementSystem.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommandManagementSystem
{
    /// <summary>
    /// An abstract standard implementation of a command
    /// </summary>
    /// <typeparam name="TParameter">The data type of the parameter to be passed</typeparam>
    /// <typeparam name="TOut">The data type of the dispatchresponse</typeparam>
    public abstract class Command<TParameter, TOut> : ICommand<TParameter, TOut>
    {
        public static MethodInfo[] ExecutionOrder { get; protected set; }
        public static bool Registered => registered && ExecutionOrder != null && ExecutionOrder?.Length > 0;
        private static bool registered;
        protected int executionCount;

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
        public virtual object TAG { get; set; }

        /// <summary>
        /// Is thrown when the command has gone through all steps
        /// </summary>
        public virtual event FinishEventHandler<TParameter> FinishEvent;
        /// <summary>
        /// Is thrown when the command waits for the next dispatch
        /// </summary>
        public virtual event WaitEventHandler<TParameter, TOut> WaitEvent;

        public Command()
        {
            executionCount = 0;
        }

        public virtual TOut Main(TParameter arg)
        {
            NextFunction = null;
            return default(TOut);
        }

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
                if (executionCount < ExecutionOrder.Length)
                    RaiseWaitEvent(this, Dispatch);
                else
                    NextFunction = null;

            if (NextFunction == null)
                RaiseFinishEvent(this, arg);

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

        public static void Registration()
        {
            var actions = MethodBase.GetCurrentMethod().DeclaringType.GetMembers()?.Where(
                m => m.GetCustomAttribute<NextAttribute>() != null)?.ToArray();

            if (actions == null || actions?.Length == 0)
                return;

            var list = new List<KeyValuePair<NextAttribute, MemberInfo>>();

            ExecutionOrder = new MethodInfo[actions.Length];

            for (int i = 0; i < actions.Length; i++)
            {
                var attr = actions[i].GetCustomAttribute<NextAttribute>();
                list.Add(new KeyValuePair<NextAttribute, MemberInfo>(attr, actions[i]));
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
    public abstract class Command : Command<EventArgs, dynamic> { }
}
