using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lucky.Hr.Core.Utility.FastInvoke
{
    public interface IFastReflectionFactory<TKey, TValue>
    {
        TValue Create(TKey key);
    }
}
