﻿using JetBrains.Annotations;

namespace Vostok.Logging.Abstractions.Values
{
    /// <summary>
    /// Represents the value of <see cref="WellKnownProperties.SourceContext"/> property.
    /// </summary>
    [PublicAPI]
    public class SourceContextValue : HierarchicalContextValue
    {
        private volatile string stringRepresentation;

        public SourceContextValue([NotNull] string[] contexts)
            : base(contexts)
        {
        }

        public SourceContextValue(string context)
            : base(context)
        {
        }

        public override string ToString() =>
            stringRepresentation ?? (stringRepresentation = string.Join(" -> ", contexts));

        public static SourceContextValue operator+([CanBeNull] SourceContextValue left, [CanBeNull] string right)
        {
            if (string.IsNullOrEmpty(right))
                return left;

            if (left == null)
                return new SourceContextValue(right);

            var newContexts = AppendToContexts(left.contexts, right);

            return ReferenceEquals(left.contexts, newContexts) ? left : new SourceContextValue(newContexts);
        }

        public static SourceContextValue operator+([CanBeNull] SourceContextValue left, [CanBeNull] SourceContextValue right)
        {
            if (right == null)
                return left;

            if (left == null)
                return right;

            return new SourceContextValue(MergeContexts(left.contexts, right.contexts));
        }
    }
}
