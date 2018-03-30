using System;

namespace CommandManagementSystem
{
    public class CommandHolder<TTag, TArgs, TReturnValue>
    {
        public Func<TArgs, TReturnValue> Delegate { get; set; }

        public TTag Tag { get; private set; }
        public TTag[] Aliases { get; internal set; }

        [Obsolete("The priority no longer has any use", false)]
        public int Priority { get; private set; }

        public CommandHolder(TTag tag)
        {
            Aliases = new TTag[0];
            Tag = tag;
        }

        public CommandHolder(TTag tag, Func<TArgs, TReturnValue> func) : this(tag) => Delegate = func;

        [Obsolete("The priority no longer has any use", false)]
        public CommandHolder(TTag tag, Func<TArgs, TReturnValue> func, int priority) : this(tag, func) => Priority = priority;

    }

    public class CommandHolder<TArgs, TReturnValue> : CommandHolder<string, TArgs, TReturnValue>
    {
        public CommandHolder(string tag) : base(tag) { }

        public CommandHolder(string tag, Func<TArgs, TReturnValue> func) : base(tag, func) { }

        [Obsolete("The priority no longer has any use", false)]
        public CommandHolder(string tag, Func<TArgs, TReturnValue> func, int priority) : base(tag, func, priority) { }

    }

    public class CommandHolder<TArgs> : CommandHolder<TArgs, dynamic>
    {
        public CommandHolder(string tag) : base(tag) { }
        public CommandHolder(string tag, Func<TArgs, dynamic> func) : base(tag, func) { }

        [Obsolete("The priority no longer has any use", false)]
        public CommandHolder(string tag, Func<TArgs, dynamic> func, int priority) : base(tag, func, priority) { }
    }

    public class CommandHolder : CommandHolder<object>
    {
        public CommandHolder(string tag) : base(tag) { }

        public CommandHolder(string tag, Func<object, dynamic> func) : base(tag, func) { }

        [Obsolete("The priority no longer has any use", false)]
        public CommandHolder(string tag, Func<object, dynamic> func, int priority) : base(tag, func, priority) { }
    }

}
