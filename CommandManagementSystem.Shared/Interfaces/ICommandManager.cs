using System.Threading.Tasks;

namespace CommandManagementSystem.Interfaces
{
    /// <summary>
    /// Interface for command managers
    /// </summary>
    /// <typeparam name="TIn">Type for the command Indetifier</typeparam>
    /// <typeparam name="TParameter">Type for the command parameters to be transferred</typeparam>
    /// <typeparam name="TOut">Return type for the dispatch</typeparam>
    public interface ICommandManager<TIn, TParameter, TOut>
    {
        /// <summary>
        /// Raises the specified command
        /// </summary>
        /// <param name="command">Command identifier</param>
        /// <param name="arg">Parameters to be passed</param>
        /// <returns>The fixed return value</returns>
        TOut Dispatch(TIn command, TParameter arg);
        /// <summary>
        /// Raises the specified command Asynchronous
        /// </summary>
        /// <param name="command">Command identifier</param>
        /// <param name="arg">Parameters to be passed</param>
        /// <returns>The fixed return value as Task</returns>
        Task<TOut> DispatchAsync(TIn command, TParameter arg);
    }
}
