using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeStorage.Logic.Interfaces
{
    public interface IDTO<TTarget, TSource> 
        where TTarget : class
        where TSource : class
    {
        public static abstract TTarget AsDTO(TSource source);
    }
}
