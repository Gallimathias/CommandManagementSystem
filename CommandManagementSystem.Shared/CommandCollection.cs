using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CommandManagementSystem
{
    internal class CommandCollection<TTag, TArgs, TReturnValue> : ICollection, IEnumerable, IDictionary
    {
        public CommandHolder<TTag, TArgs, TReturnValue> this[TTag commandName]
        {
            get
            {
                TryGetValue(commandName, out CommandHolder<TTag, TArgs, TReturnValue> value);
                return value;
            }
            set
            {
                TryAdd(value);
            }
        }
        public CommandHolder<TTag, TArgs, TReturnValue> this[int index]
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
                if (key is TTag tag)
                    return this[tag];
                else if (key is int index)
                    return this[index];
                else
                    return null;
            }
            set
            {
                if (key is TTag tag)
                    this[tag] = (CommandHolder<TTag, TArgs, TReturnValue>)value;
                else if (key is int index)
                    this[index] = (CommandHolder<TTag, TArgs, TReturnValue>)value;
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

        private Dictionary<TTag, int> keys;
        private List<CommandHolder<TTag, TArgs, TReturnValue>> items;
        private Queue<int> freeSpace;
        private readonly object itemsLock;
        private readonly object keysLock;
        private int globalIndex;

        public CommandCollection()
        {
            keys = new Dictionary<TTag, int>();
            items = new List<CommandHolder<TTag, TArgs, TReturnValue>>();
            freeSpace = new Queue<int>();
            itemsLock = new object();
            keysLock = new object();
            globalIndex = 0;
        }

        public bool TryAdd(CommandHolder<TTag, TArgs, TReturnValue> commandHolder)
        {
            int freeIndex;

            lock (keysLock)
            {
                if (keys.ContainsKey(commandHolder.Tag))
                    return false;
            }

            lock (itemsLock)
            {
                if (freeSpace.Count == 0)
                {
                    freeIndex = globalIndex;
                    items.Add(commandHolder);
                    globalIndex++;
                }
                else
                {
                    freeIndex = freeSpace.Dequeue();
                    items[freeIndex] = commandHolder;
                }
            }

            lock (keysLock)
            {
                keys.Add(commandHolder.Tag, freeIndex);

                for (int i = 0; i < commandHolder.Aliases.Length; i++)
                    keys.Add(commandHolder.Aliases[i], freeIndex);
            }

            return true;
        }

        public bool TryUpdate(CommandHolder<TTag, TArgs, TReturnValue> commandHolder)
        {
            bool keyExist;
            int index;

            lock (keysLock)
            {
                keyExist = keys.TryGetValue(commandHolder.Tag, out index);
            }

            if (keyExist)
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

        public IEnumerable<KeyValuePair<TTag, TTag[]>> GetTagList()
        {
            lock (itemsLock)
                return items.Select(i => new KeyValuePair<TTag, TTag[]>(i.Tag, i.Aliases));
        }

        public bool TryUpdate(TTag tag, Func<TArgs, TReturnValue> action)
        {
            int index;

            lock (keysLock)
                if (!keys.TryGetValue(tag, out index))
                    return false;

            lock (itemsLock)
                items[index].Delegate = action;

            return true;
        }

        public bool TryGetValue(TTag tag, out CommandHolder<TTag, TArgs, TReturnValue> commandHolder)
        {
            bool keyResult;
            int index;
            commandHolder = null;

            lock (keysLock)
                keyResult = keys.TryGetValue(tag, out index);

            if (keyResult)
            {
                lock (itemsLock)
                    commandHolder = items[index];
            }

            return keyResult;
        }

        public bool ContainsKey(TTag commandName)
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
            => GetTagList().GetEnumerator();

        public void Add(object key, object value)
            => throw new NotSupportedException("Please use TryAdd instead");

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
                return keys.ContainsKey((TTag)key);
        }

        public void Remove(object key)
        {
            int index;
            lock (keysLock)
                index = keys[(TTag)key];

            lock (itemsLock)
                items[index] = null;

            lock (keysLock)
            {
                foreach (var tmpKey in keys.Where(k => k.Value == index).Select(k => k.Key))
                    keys.Remove(tmpKey);
            }

            lock (itemsLock)
                freeSpace.Enqueue(index);
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            lock (keysLock)
                return keys.GetEnumerator();
        }
    }
}
