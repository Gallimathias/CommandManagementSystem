using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace CommandManagementSystem
{
    internal class CommandList<TId, TArgs, TReturnValue> : ICollection, IEnumerable, IDictionary
    {
        public CommandHolder<TId, TArgs, TReturnValue> this[TId commandName]
        {
            get
            {
                TryGetValue(commandName, out CommandHolder<TId, TArgs, TReturnValue> value);
                return value;
            }
            set
            {
                TryAdd(value);
            }
        }
        public CommandHolder<TId, TArgs, TReturnValue> this[int index]
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
                if (key is TId id)
                    return this[id];
                else if (key is int index)
                    return this[index];
                else
                    return null;
            }
            set
            {
                if (key is TId id)
                    this[id] = (CommandHolder<TId, TArgs, TReturnValue>)value;
                else if (key is int index)
                    this[index] = (CommandHolder<TId, TArgs, TReturnValue>)value;
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
        
        private Dictionary<TId, int> keys;
        private List<CommandHolder<TId, TArgs, TReturnValue>> items;
        private object itemsLock;
        private object keysLock;
        private int globalIndex;

        public CommandList()
        {
            keys = new Dictionary<TId, int>();
            items = new List<CommandHolder<TId, TArgs, TReturnValue>>();
            itemsLock = new object();
            keysLock = new object();
            globalIndex = 0;
        }

        public bool TryAdd(CommandHolder<TId, TArgs, TReturnValue> commandHolder)
        {
            lock (keysLock)
            {
                if (keys.ContainsKey(commandHolder.Id))
                    return false;

                lock (itemsLock)
                {
                    items.Add(commandHolder);

                    keys.Add(commandHolder.Id, globalIndex);

                    for (int i = 0; i < commandHolder.Aliases.Length; i++)
                        keys.Add(commandHolder.Aliases[i], globalIndex);

                    globalIndex++;
                }
            }

            return true;
        }

        public bool TryUpdate(CommandHolder<TId, TArgs, TReturnValue> commandHolder)
        {
            lock (keysLock)
            {
                if(keys.TryGetValue(commandHolder.Id, out int index))
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

        public bool TryGetValue(TId id, out CommandHolder<TId, TArgs, TReturnValue> commandHolder)
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

        public bool ContainsKey(TId commandName)
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
                    globalIndex = 0;
                }
            }
        }

        public bool Contains(object key)
        {
            lock (keysLock)
                return keys.ContainsKey((TId)key);
        }

        public void Remove(object key)
        {
            lock (keysLock)
            {
                lock (itemsLock)
                {
                    var index = keys[(TId)key];
                    items.RemoveAt(index);

                    keys.Clear();

                    for (globalIndex = 0; globalIndex < items.Count; globalIndex++)
                    {
                        keys.Add(items[globalIndex].Id, globalIndex);

                        for (int a = 0; a < items[globalIndex].Aliases.Length; a++)
                            keys.Add(items[globalIndex].Aliases[a], globalIndex);
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
