using System;
using System.Collections.Generic;
using System.Text;

namespace CommandManagementSystem.Attributes
{
    /// <summary>
    /// Indicates a class as a command with there name as identifier
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true)]
    public class StringCommandAttribute : Attribute
    {
        /// <summary>
        /// Alternate names for the command
        /// </summary>
        public readonly string[] Aliases;
        /// <summary>
        /// Returns a true if the command is to be reinitialized
        /// </summary>
        public bool Reinitialize { get; internal set; }

        /// <summary>
        /// Indicates a class as a command
        /// </summary>
        /// <param name="tag">Uniquely identifies the command</param>
        /// <param name="aliases">Alternate names for the command</param>
        public StringCommandAttribute(params string[] aliases)
        {
            Aliases = aliases;
            Reinitialize = true;
        }
        /// <summary>
        /// Indicates a class as a command
        /// </summary>
        /// <param name="tag">Uniquely identifies the command</param>
        /// <param name="reinitialize">Specifies whether the command is to be reinitialized.</param>
        /// <param name="aliases">Alternate names for the command</param>
        public StringCommandAttribute(bool reinitialize, params string[] aliases)
            : this(aliases)
        {
            Reinitialize = reinitialize;
        }
    }
}
