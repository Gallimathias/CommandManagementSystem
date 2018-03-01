using System;

namespace CommandManagementSystem
{
    internal class CommandHolder<TId, TArgs, TReturnValue>
    {
        public Func<TArgs, TReturnValue> Delegate { get; set; }

        public TId Id { get; private set; }
        public TId[] Aliases { get; internal set; }

        [Obsolete("The priority no longer has any use", false)]
        public int Priority { get; private set; }

        public CommandHolder(TId id)
        {
            Aliases = new TId[0];
            Id = id;
        }

        public CommandHolder(TId id, Func<TArgs, TReturnValue> func) : this(id) => Delegate = func;

        [Obsolete("The priority no longer has any use", false)]
        public CommandHolder(TId id, Func<TArgs, TReturnValue> func, int priority) : this(id, func) => Priority = priority;

    }

    internal class CommandHolder<TArgs, TReturnValue> : CommandHolder<string, TArgs, TReturnValue>
    {
        public CommandHolder(string id) : base(id) { }

        public CommandHolder(string id, Func<TArgs, TReturnValue> func) : base(id, func) { }

        [Obsolete("The priority no longer has any use", false)]
        public CommandHolder(string id, Func<TArgs, TReturnValue> func, int priority) : base(id, func, priority) { }

    }

    internal class CommandHolder<TArgs> : CommandHolder<TArgs, dynamic>
    {
        public CommandHolder(string id) : base(id) { }
        public CommandHolder(string id, Func<TArgs, dynamic> func) : base(id, func) { }

        [Obsolete("The priority no longer has any use", false)]
        public CommandHolder(string id, Func<TArgs, dynamic> func, int priority) : base(id, func, priority) { }
    }

    internal class CommandHolder : CommandHolder<object>
    {
        public CommandHolder(string id) : base(id) { }

        public CommandHolder(string id, Func<object, dynamic> func) : base(id, func) { }

        [Obsolete("The priority no longer has any use", false)]
        public CommandHolder(string id, Func<object, dynamic> func, int priority) : base(id, func, priority) { }
    }

}
