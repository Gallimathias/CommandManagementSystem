using System;

namespace CommandManagementSystem.Attributes
{
    /// <summary>
    /// Indicates a class as a command
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true)]
    public class CommandAttribute : Attribute
    {
        /// <summary>
        /// Uniquely identifies the command
        /// </summary>
        public readonly object Tag;
        /// <summary>
        /// Alternate names for the command
        /// </summary>
        public readonly object[] Aliases;
        /// <summary>
        /// Returns a true if the command is to be reinitialized
        /// </summary>
        public bool Reinitialize { get; internal set; }

        /// <summary>
        /// Indicates a class as a command
        /// </summary>
        /// <param name="tag">Uniquely identifies the command</param>
        /// <param name="aliases">Alternate names for the command</param>
        public CommandAttribute(object tag, params object[] aliases)
        {
            Tag = tag;
            Aliases = aliases;
            Reinitialize = true;
        }
        /// <summary>
        /// Indicates a class as a command
        /// </summary>
        /// <param name="tag">Uniquely identifies the command</param>
        /// <param name="reinitialize">Specifies whether the command is to be reinitialized.</param>
        /// <param name="aliases">Alternate names for the command</param>
        public CommandAttribute(object tag, bool reinitialize, params object[] aliases) 
            : this(tag, aliases)
        {
            Reinitialize = reinitialize;
        }
    }
}
