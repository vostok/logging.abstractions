using System;

namespace Vostok.Logging.Abstractions.Helpers
{
    internal static class TypesHelper
    {
        public static bool IsConstructedGenericType(Type type) =>
            type.IsConstructedGenericType && Nullable.GetUnderlyingType(type) == null;
    }
}