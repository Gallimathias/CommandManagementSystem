using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace CommandManagementSystem
{
    internal class CommandList<TID, TArgs, TReturnValue>
    {
        public CommandHolder<TID, TArgs, TReturnValue> this[TID commandName]
        {
            get
            {
                TryGetValue(commandName, out CommandHolder<TID, TArgs, TReturnValue> value);
                return value;
            }
            set
            {


            }
        }
        public CommandHolder<TID, TArgs, TReturnValue> this[int index]
        {
            get
            {
                lock (itemsLock)
                    return items[index];
            }
            set
            {
                lock (itemsLock)
                    items[index] = value;
            }
        }

        private Dictionary<TID, int> keys;
        private List<CommandHolder<TID, TArgs, TReturnValue>> items;
        private object itemsLock;
        private object keysLock;

        public CommandList()
        {
            keys = new Dictionary<TID, int>();
            items = new List<CommandHolder<TID, TArgs, TReturnValue>>();
            itemsLock = new object();
            keysLock = new object();
        }

        public bool TryAdd(CommandHolder<TID, TArgs, TReturnValue> commandHolder)
        {
            lock (keysLock)
            {
                if (keys.ContainsKey(commandHolder.ID))
                    return false;

                lock (itemsLock)
                {
                    items.Add(commandHolder);

                    keys.Add(commandHolder.ID, items.Count);

                    for (int i = 0; i < commandHolder.Aliases.Length; i++)
                        keys.Add(commandHolder.Aliases[i], items.Count);
                }
            }

            return true;
        }

        public bool TryGetValue(TID id, out CommandHolder<TID, TArgs, TReturnValue> commandHolder)
        {
            bool keyResult;
            int index;
            commandHolder = null;

            lock (keysLock)
                keyResult = keys.TryGetValue(id, out index);

            if (keyResult)
            {
                lock (itemsLock)
                    commandHolder = items[index];
            }

            return keyResult;
        }
    }
}
