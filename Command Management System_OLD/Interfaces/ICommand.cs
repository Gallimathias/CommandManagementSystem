using System;

namespace CommandManagementSystem.Interfaces
{
    /// <summary>
    /// Delegate for command waiting events
    /// </summary>
    /// <typeparam name="TParameter">Parameters type for the command</typeparam>
    /// <typeparam name="TOut">Return value type of the command</typeparam>
    /// <param name="sender">The command as object</param>
    /// <param name="arg">Parameters for the command</param>
    public delegate void WaitEventHandler<TParameter, TOut>(object sender, Func<TParameter, TOut> arg);
    /// <summary>
    /// Delegate for command finish events
    /// </summary>
    /// <typeparam name="TParameter">Parameters type for the command</typeparam>
    /// <param name="sender">The command as object</param>
    /// <param name="arg">Parameters for the command</param>
    public delegate void FinishEventHandler<TParameter>(object sender, TParameter arg);

    /// <summary>
    /// Interface for commands
    /// </summary>
    /// <typeparam name="TParameter">Parameters type for the command</typeparam>
    /// <typeparam name="TOut">Return value type of the command</typeparam>
    public interface ICommand<TParameter, TOut>
    {
        /// <summary>
        /// Contains the delegates for the next function to execute
        /// </summary>
        Func<TParameter, TOut> NextFunction { get; }
        /// <summary>
        /// Returns whether the command has already gone through all steps
        /// </summary>
        bool Finished { get; }
        /// <summary>
        /// Unique Indentifikator for the command
        /// </summary>
        object TAG { get; }

        /// <summary>
        /// Is thrown when the command waits for the next dispatch
        /// </summary>
        event WaitEventHandler<TParameter, TOut> WaitEvent;
        /// <summary>
        /// Is thrown when the command has gone through all steps
        /// </summary>
        event FinishEventHandler<TParameter> FinishEvent;

        /// <summary>
        /// The main method is executed when dispatch if no Dispatch Order attribute found in the class.
        /// </summary>
        /// <param name="arg">The arguments passed by the dispatch</param>
        /// <returns>Returns the result of the command</returns>
        TOut Main(TParameter arg);

        /// <summary>
        /// Raises the wait event
        /// </summary>
        /// <param name="sender">This command</param>
        /// <param name="arg">The dispatch method</param>
        void RaiseWaitEvent(object sender, Func<TParameter, TOut> arg);
        /// <summary>
        /// Raises the finish event
        /// </summary>
        /// <param name="sender">This command</param>
        /// <param name="arg">The passed parameters</param>
        void RaiseFinishEvent(object sender, TParameter arg);

        /// <summary>
        /// Executes the next action in the command
        /// </summary>
        /// <param name="arg">Parameters to be passed</param>
        /// <returns>Returns a fixed return value</returns>
        TOut Dispatch(TParameter arg);
        /// <summary>
        /// Initializes the command
        /// </summary>
        /// <param name="arg">Parameters to be passed</param>
        /// <returns>Returns a fixed return value</returns>
        TOut Initialize(TParameter arg);
    }
}