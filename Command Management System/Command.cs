using CoMaS.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoMaS
{
    public abstract class Command<TParameter, TOut> : ICommand<TParameter, TOut>
    {
        public Func<TParameter, TOut> NextFunction { get; set; }
        public bool Finished { get; protected set; }
        public virtual object TAG { get; set; }

        public virtual event FinishEventHandler<TParameter> FinishEvent;
        public virtual event WaitEventHandler<TParameter, TOut> WaitEvent;

        public virtual TOut Dispatch(TParameter arg) => NextFunction(arg);

        public virtual TOut Initialize(TParameter arg) => Dispatch(arg);
            
        public virtual void RaiseFinishEvent(object sender, TParameter arg) => FinishEvent?.Invoke(sender, arg);
        public virtual void RaiseWaitEvent(object sender, Func<TParameter, TOut> arg) => WaitEvent?.Invoke(sender, arg);
    }

    public abstract class Command<TParameter> : Command<TParameter, dynamic> { }

    public abstract class Command : Command<EventArgs, dynamic> { }
}
