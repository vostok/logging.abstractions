using System.Linq;
using System.Runtime.CompilerServices;

namespace Vostok.Logging.Abstractions.Helpers
{
    internal static class InterpolatedHelper
    {
        private const char Underscore = '_';
        private const char At = '@';
        private const char Dot = '.';
        
        public static string EscapeName(string name)
        {
            if (name.All(IsValidInName))
                return name;

            var copy = name.ToCharArray();
            for (var i = 0; i < copy.Length; i++)
                if (!IsValidInName(copy[i]))
                    copy[i] = '_';
            return new string(copy);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsValidInName(char c) =>
            char.IsLetterOrDigit(c) || c == Underscore || c == At || c == Dot;
    }
}