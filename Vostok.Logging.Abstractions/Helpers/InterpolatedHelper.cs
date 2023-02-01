#if NET6_0_OR_GREATER
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace Vostok.Logging.Abstractions.Helpers
{
    internal static class InterpolatedHelper
    {
        private const char Underscore = '_';
        private const char At = '@';
        private const char Dot = '.';

        public static bool IsValidName(string name)
        {
            // ReSharper disable once ForCanBeConvertedToForeach
            // ReSharper disable once LoopCanBeConvertedToQuery
            for (var i = 0; i < name.Length; i++)
            {
                if (!IsValidInName(name[i]))
                    return false;
            }

            return true;
        }

        // note (kungurtsev, 25.01.2022): copied from Vostok.Logging.Formatting.Tokenizer.TemplateTokenizer
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsValidInName(char c) =>
            char.IsLetterOrDigit(c) || c == Underscore || c == At || c == Dot;
    }
}
#endif