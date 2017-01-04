﻿using CoMaS.Attributes;
using CoMaS.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CoMaS
{
    public abstract class CommandManager<TIn, TParameter, TOut> : ICommandManager<TIn, TParameter, TOut>
    {
        protected CommandHandler<TIn, TParameter, TOut> commandHandler;
        protected ConcurrentDictionary<TIn, Func<TParameter, TOut>> waitingDictionary;

        public delegate void CommandManagerEventHandler(ICommand<TParameter, TOut> command, TParameter arg);
        public virtual event CommandManagerEventHandler OnFinishedCommand;
        public virtual event CommandManagerEventHandler OnWaitingCommand;

        public CommandManager()
        {
            commandHandler = new CommandHandler<TIn, TParameter, TOut>();
            waitingDictionary = new ConcurrentDictionary<TIn, Func<TParameter, TOut>>();
            Initialize();
        }

        public virtual void Initialize()
        {
            var commandNamespace = GetType().GetCustomAttribute<CommandManagerAttribute>().CommandNamespaces;

            var commands = Assembly.GetExecutingAssembly().GetTypes().Where(
                t => t.GetCustomAttribute<CommandAttribute>() != null && commandNamespace.Contains(t.Namespace)).ToList();

            foreach (var command in commands)
            {
                commandHandler[(TIn)command.GetCustomAttribute<CommandAttribute>().Tag] += (e)
                    => InitializeCommand(command, e);
            }
        }

        public virtual TOut Dispatch(TIn command, TParameter arg) =>
            commandHandler.Dispatch(command, arg);

        public virtual async Task<TOut> DispatchAsync(TIn command, TParameter arg) =>
            await Task.Run(() => Dispatch(command, arg));

        public virtual TOut InitializeCommand(ICommand<TParameter, TOut> command, TParameter arg)
        {
            command.FinishEvent += Command_FinishEvent;
            command.WaitEvent += Command_WaitEvent;

            return command.Initialize(arg);
        }
        public virtual TOut InitializeCommand(Type commandType, TParameter arg)
        {
            var command = (ICommand<TParameter, TOut>)Activator.CreateInstance(commandType);
            return InitializeCommand(command, arg);
        }
        public virtual TOut InitializeCommand(Type commandType, TParameter arg, params object[] startParams)
        {
            var command = (ICommand<TParameter, TOut>)Activator.CreateInstance(commandType, startParams);
            return InitializeCommand(command, arg);
        }

        public virtual void Command_FinishEvent(object sender, TParameter arg)
        {
            Func<TParameter, TOut> method;
            var command = (ICommand<TParameter, TOut>)sender;
            waitingDictionary.TryRemove((TIn)command.TAG, out method);
            OnFinishedCommand?.Invoke(command, arg);
        }

        public virtual void Command_WaitEvent(object sender, Func<TParameter, TOut> arg)
        {
            if (arg == null && sender == null)
                return;
            var command = (ICommand<TParameter, TOut>)sender;

            if (!waitingDictionary.TryAdd((TIn)command.TAG, arg))
                waitingDictionary.TryUpdate((TIn)command.TAG, arg, arg);
        }
    }
}
