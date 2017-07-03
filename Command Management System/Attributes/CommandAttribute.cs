﻿using System;

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
        public readonly string[] Aliases;

        /// <summary>
        /// Indicates a class as a command
        /// </summary>
        /// <param name="tag">Uniquely identifies the command</param>
        /// <param name="aliases">Alternate names for the command</param>
        public CommandAttribute(object tag, params string[] aliases)
        {
            Tag = tag;
            Aliases = aliases;
        }
    }
}
