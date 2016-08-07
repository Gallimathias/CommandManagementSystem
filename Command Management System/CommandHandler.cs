using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoMaS
{
    public class CommandHandler<TIn, TParameter, TOut>
    {
        public Queue<KeyValuePair<TIn,TParameter>> CommandQueue { get; private set; }

        private Dictionary<TIn, CommandHolder<TIn, TParameter, TOut>> mainDictionary;
        

        public CommandHandler()
        {
            mainDictionary = new Dictionary<TIn, CommandHolder<TIn, TParameter, TOut>>();
            CommandQueue = new Queue<KeyValuePair<TIn, TParameter>>();
        }

        public Func<TParameter, TOut> this[TIn commandName]
        {
            get
            {
                CommandHolder<TIn, TParameter, TOut> value;
                mainDictionary.TryGetValue(commandName, out value);
                return value.Delegate as Func<TParameter, TOut>;
            }
            set
            {
                if (mainDictionary.ContainsKey(commandName))
                    mainDictionary[commandName] = new CommandHolder<TIn, TParameter, TOut>(commandName, value);
                else
                    mainDictionary.Add(commandName, new CommandHolder<TIn, TParameter, TOut>(commandName, value));

            }
        }

        public TOut Dispatch(TIn commandName, TParameter parameter) => mainDictionary[commandName].Delegate(parameter);

        public void DispatchOnSubmit(TIn commandName, TParameter parameter) => CommandQueue.Enqueue(new KeyValuePair<TIn, TParameter>(commandName, parameter));

        public TOut Submit()
        {
            var list = CommandQueue.ToList();
            CommandQueue = new Queue<KeyValuePair<TIn, TParameter>>();
            return internalSubmit(list);
        }
        

        public bool CommandExists(TIn commandName) => mainDictionary.ContainsKey(commandName);

        private TOut internalSubmit(List<KeyValuePair<TIn, TParameter>> commands)
        {
            TOut returnValue = default(TOut);

            foreach (var command in commands)
            {
                returnValue = Dispatch(command.Key, command.Value);
            }

            return returnValue;
        }
    }

    public class CommandHandler<TParameter, TOut> : CommandHandler<string, TParameter, TOut> { }

    public class CommandHandler<TParameter> : CommandHandler<TParameter, dynamic> { }

    public class CommandHandler : CommandHandler<EventArgs> { }
}
