using System;

namespace CoMaS.Interfaces
{
    public delegate void WaitEventHandler<TParameter, TOut>(object sender, Func<TParameter, TOut> arg);
    public delegate void FinishEventHandler<TParameter>(object sender, TParameter arg);
    public interface ICommand<TParameter, TOut>
    {
        Func<TParameter, TOut> NextFunction { get; }
        bool Finished { get; }
        object TAG { get; set; }

        event WaitEventHandler<TParameter, TOut> WaitEvent;
        event FinishEventHandler<TParameter> FinishEvent;

        void RaiseWaitEvent(object sender, Func<TParameter, TOut> arg);
        void RaiseFinishEvent(object sender, TParameter arg);

        TOut Dispatch(TParameter arg);
        TOut Initialize(TParameter arg);
    }
}