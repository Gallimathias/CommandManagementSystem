using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace CommandManagementSystem
{
    internal class CommandList<TID, TArgs, TReturnValue> : ICollection, IEnumerable, IDictionary
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
                TryAdd(value);
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
        public object this[object key]
        {
            get
            {
                if (key is TID id)
                    return this[id];
                else if (key is int index)
                    return this[index];
                else
                    return null;
            }
            set
            {
                if (key is TID id)
                    this[id] = (CommandHolder<TID, TArgs, TReturnValue>)value;
                else if (key is int index)
                    this[index] = (CommandHolder<TID, TArgs, TReturnValue>)value;
            }
        }
        public int Count
        {
            get
            {
                lock (itemsLock)
                    return items.Count;
            }
        }
        
        public bool IsSynchronized => false;
        public object SyncRoot => throw new NotImplementedException();
        public bool IsFixedSize => false;
        public bool IsReadOnly => false;

        public ICollection Keys
        {
            get
            {
                lock (keysLock)
                    return keys.Keys;
            }
        }
        public ICollection Values
        {
            get
            {
                lock (itemsLock)
                    return items.ToArray();
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

        public bool TryUpdate(CommandHolder<TID, TArgs, TReturnValue> commandHolder)
        {
            lock (keysLock)
            {
                if(keys.TryGetValue(commandHolder.ID, out int index))
                {
                    lock (itemsLock)
                        items[index] = commandHolder;

                    return true;
                }
                else
                {
                    return false;
                }
            }
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

        public bool ContainsKey(TID commandName)
        {
            lock (keysLock)
                return keys.ContainsKey(commandName);
        }

        public void CopyTo(Array array, int index)
        {
            for (int i = index; i < array.Length; i++)
            {
                lock (itemsLock)
                {
                    for (int a = 0; a < items.Count; a++)
                        array.SetValue(items[a], i);
                }
            }
        }

        public IEnumerator GetEnumerator()
        {
            lock (itemsLock)
                return items.GetEnumerator();
        }

        public void Add(object key, object value) => throw new NotSupportedException("Please use TryAdd instead");

        public void Clear()
        {
            lock (itemsLock)
            {
                lock (keysLock)
                {
                    items.Clear();
                    keys.Clear();
                }
            }
        }

        public bool Contains(object key)
        {
            lock (keysLock)
                return keys.ContainsKey((TID)key);
        }

        public void Remove(object key)
        {
            lock (keysLock)
            {
                lock (itemsLock)
                {
                    var index = keys[(TID)key];
                    items.RemoveAt(index);

                    keys.Clear();

                    for (int i = 0; i < items.Count; i++)
                    {
                        keys.Add(items[i].ID, i);

                        for (int a = 0; a < items[i].Aliases.Length; a++)
                            keys.Add(items[i].Aliases[a], i);
                    }
                }
            }
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            lock (keysLock)
                return keys.GetEnumerator();
        }
    }
}
