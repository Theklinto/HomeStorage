using HomeStorage.Logic.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace HomeStorage.Logic.Services
{
    internal static class DTOService
    {
        internal static TTarget AsDTO<TTarget, TSource>([NotNullIfNotNull(nameof(source))] TSource source)
            where TTarget : class, IDTO<TTarget, TSource>
            where TSource : class
        {
            return TTarget.AsDTO(source);
        }
    }
}
