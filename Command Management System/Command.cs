using CoMaS.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoMaS
{
    /// <summary>
    /// An abstract standard implementation of a command
    /// </summary>
    /// <typeparam name="TParameter">The data type of the parameter to be passed</typeparam>
    /// <typeparam name="TOut">The data type of the dispatchresponse</typeparam>
    public abstract class Command<TParameter, TOut> : ICommand<TParameter, TOut>
    {
        /// <summary>
        /// Contains the delegates for the next function to execute
        /// </summary>
        public Func<TParameter, TOut> NextFunction { get; set; }
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

        /// <summary>
        /// Executes the next action in the command
        /// </summary>
        /// <param name="arg">Parameters to be passed</param>
        /// <returns>Returns a fixed return value</returns>
        public virtual TOut Dispatch(TParameter arg) => NextFunction(arg);

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
