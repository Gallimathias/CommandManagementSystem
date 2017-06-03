using System;

namespace CommandManagementSystem.Attributes
{
    /// <summary>
    /// Indicates a method as a command
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class OneTimeCommandAttribute : CommandAttribute
    {
        /// <summary>
        /// Indicates a method as a command
        /// </summary>
        /// <param name="tag">Uniquely identifies the command</param>
        /// <param name="aliases">Alternate names for the command</param>
        public OneTimeCommandAttribute(object tag, params string[] aliases) : base(tag, aliases)
        {
        }
    }
}
