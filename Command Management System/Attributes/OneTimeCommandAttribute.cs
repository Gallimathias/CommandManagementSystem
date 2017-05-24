using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandManagementSystem.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class OneTimeCommandAttribute : CommandAttribute
    {
        public OneTimeCommandAttribute(object tag, params string[] aliases) : base(tag, aliases)
        {
        }
    }
}
