using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoMaS.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class CommandAttribute : Attribute
    {
        public readonly object Tag;
        public readonly string[] Aliases;

        public CommandAttribute(object tag, params string[] aliases)
        {
            Tag = tag;
            Aliases = aliases;
        }
    }
}
