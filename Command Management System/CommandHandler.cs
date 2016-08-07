using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoMaS
{
    public class CommandHandler<TIn, TParameter, TOut>
    {
        private Dictionary<TIn, Func<TParameter, TOut>> mainDictionary;

        public CommandHandler()
        {
            mainDictionary = new Dictionary<TIn, Func<TParameter, TOut>>();
        }

        public Func<TParameter, TOut> this[TIn commandName]
        {
            get
            {
                Func<TParameter, TOut> value;
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

        public TOut Dispatch(TIn commandName, TParameter parameter) => mainDictionary[commandName](parameter);

        public bool CommandExists(TIn commandName) => mainDictionary.ContainsKey(commandName);
    }

    public class CommandHandler<TParameter, TOut> : CommandHandler<string, TParameter, TOut> { }

    public class CommandHandler<TParameter> : CommandHandler<TParameter, dynamic> { }

    public class CommandHandler : CommandHandler<EventHandler> { }
}
