using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeStorage.Interfaces
{
    public interface IExtendModel<T> where T : class
    {
        public T GetBase();
        public void SetBase(T source);
    }
}
