using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoMaS.Interfaces
{
    public interface ICommandManager<TIn, TParameter, TOut>
    {
        TOut Dispatch(TIn command, TParameter arg);
        Task<TOut> DispatchAsync(TIn command, TParameter arg);
    }
}
