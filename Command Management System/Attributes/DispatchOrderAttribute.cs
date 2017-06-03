using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandManagementSystem.Attributes
{
    /// <summary>
    /// Registers and defines a method in a Command class for execution when dispatching
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class DispatchOrderAttribute : Attribute
    {
        /// <summary>
        /// After this property, the class sorts the Registered methods. 
        /// Higher numbers are executed later than lower.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Registers and defines a method in a Command class for execution when dispatching
        /// </summary>
        /// <param name="order">The order of the method</param>
        public DispatchOrderAttribute(int order) => Order = order;
        /// <summary>
        /// Registers and defines a method in a Command class for execution when dispatching.
        /// Sets the sequence to 0.
        /// </summary>
        public DispatchOrderAttribute() : this(0) { }
    }
}
