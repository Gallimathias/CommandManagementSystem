using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoMaS
{
    public class CommandHandler<TIn, TOut>
    {
        private Dictionary<string, Func<TIn, TOut>> mainDictionary;
        
        public CommandHandler()
        {
            mainDictionary = new Dictionary<string, Func<TIn, TOut>>();
        }

        public Func<TIn, TOut> this[string commandName]
        {
            get
            {
                Func<TIn, TOut> value;
                mainDictionary.TryGetValue(commandName, out value);
                return value;
            }
            set
            {
                if (mainDictionary.ContainsKey(commandName))
                    mainDictionary[commandName] = value;
                else
                    mainDictionary.Add(commandName, value);

            }
        }

        public TOut Dispatch(string commandName, TIn parameter) => mainDictionary[commandName](parameter);

        public bool CommandExists(string commandName) => mainDictionary.ContainsKey(commandName);
    }
}
