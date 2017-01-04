using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoMaS.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class CommandManagerAttribute : Attribute
    {
        public readonly object Tag;
        public readonly string[] CommandNamespaces;

        public CommandManagerAttribute(object tag)
        {
            Tag = tag;
        }

        public CommandManagerAttribute(object tag, params string[] commandNamespaces)
        {
            Tag = tag;
            CommandNamespaces = commandNamespaces;
        }
    }
}
