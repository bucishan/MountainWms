using System.Collections.Generic;

namespace M.NetCore.Autofac
{
    public interface IResolver
    {
        T Resolve<T>();

        IEnumerable<T> ResolveAll<T>();
    }
}