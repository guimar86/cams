using System;

namespace cams.api.Mappers;

public static class EnumMapper
{
    public static TTarget MapEnumByName<TSource, TTarget>(TSource source)
        where TSource : Enum
        where TTarget : struct, Enum
    {
        string name = source.ToString();
        if (Enum.TryParse(typeof(TTarget), name, ignoreCase: false, out var result) && result is TTarget targetValue)
        {
            return targetValue;
        }

        throw new ArgumentException(
            $"Cannot map value '{name}' from {typeof(TSource).Name} to {typeof(TTarget).Name}.");
    }
}


