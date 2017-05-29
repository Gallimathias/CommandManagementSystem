using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandManagementSystem.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class DispatchOrderAttribute : Attribute
    {
        public int Order { get; set; }

        public DispatchOrderAttribute(int order) => Order = order;
        public DispatchOrderAttribute() : this(0) { }
    }
}
