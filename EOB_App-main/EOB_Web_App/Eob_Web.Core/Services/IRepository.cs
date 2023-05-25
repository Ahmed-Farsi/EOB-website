using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eob_Web.Core.Services
{
    public interface IRepository
    {
        Task<IEnumerable<T>> Load<T, U>(string procedure, U parameters);
        Task<IEnumerable<TReturn>> Load_Multi<T1, T2, T3, T4, TReturn, U>(string procedure, U parameters, Func<T1, T2, T3, T4, TReturn> mapping);
        Task<IEnumerable<TReturn>> Load_Multi<T1, T2, T3, TReturn, U>(string procedure, U parameters, Func<T1, T2, T3, TReturn> mapping);
        Task<IEnumerable<TReturn>> Load_Multi<T1, T2, TReturn, U>(string procedure, U parameters, Func<T1, T2, TReturn> mapping);
        Task<int> Modify<T>(string procedure, T parameters);
    }
}