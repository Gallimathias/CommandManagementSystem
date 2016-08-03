using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoMaS
{
    public class Command<TParameter, TOut>
    {
        public Func<TParameter, TOut> NextFunction { get; set; }

        public TOut Dispatch(TParameter parameter) => NextFunction(parameter);

        public delegate void FinishEventHandler(object sender, TParameter e);

        public event FinishEventHandler FinishEvent;

        public void RaiseFinishEvent(object sender, TParameter e) => FinishEvent?.Invoke(sender, e);

    }
}
