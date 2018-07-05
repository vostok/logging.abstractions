using System;
// ReSharper disable CheckNamespace

namespace JetBrains.Annotations
{
    [AttributeUsage(
        AttributeTargets.Constructor | AttributeTargets.Method |
        AttributeTargets.Property | AttributeTargets.Delegate)]
    internal sealed class StringFormatMethodAttribute : Attribute
    {
        public StringFormatMethodAttribute([NotNull] string formatParameterName)
        {
            FormatParameterName = formatParameterName;
        }

        [NotNull]
        public string FormatParameterName { get; private set; }
    }
}