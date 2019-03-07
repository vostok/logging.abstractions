using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace Vostok.Logging.Abstractions.Values
{
    /// <summary>
    /// Represents the value of <see cref="WellKnownProperties.OperationContext"/> property.
    /// </summary>
    [PublicAPI]
    public class OperationContextValue : HierarchicalContextValue
    {
        private volatile string stringRepresentation;

        public OperationContextValue([NotNull] string[] contexts)
            : base(contexts)
        {
        }

        public OperationContextValue(string context)
            : base(context)
        {
        }

        public override string ToString() =>
            stringRepresentation ?? (stringRepresentation = ToStringInternal());

        public static OperationContextValue operator+([CanBeNull] OperationContextValue left, [CanBeNull] string right)
        {
            if (string.IsNullOrEmpty(right))
                return left;

            if (left == null)
                return new OperationContextValue(right);

            var newContexts = AppendToContexts(left.contexts, right);

            return ReferenceEquals(left.contexts, newContexts) ? left : new OperationContextValue(newContexts);
        }

        private string ToStringInternal()
        {
            var builder = new StringBuilder(contexts.Sum(c => c.Length) + contexts.Length * 3);

            for (var index = 0; index < contexts.Length; index++)
            {
                builder
                    .Append('[')
                    .Append(contexts[index])
                    .Append(']');

                if (index < contexts.Length - 1)
                    builder.Append(' ');
            }

            return builder.ToString();
        }

        #region Equality

        public bool Equals(OperationContextValue other)
            => ReferenceEquals(this, other) || other != null && contexts.SequenceEqual(other.contexts);

        public override bool Equals(object other)
            => Equals(other as OperationContextValue);

        public override int GetHashCode()
            => contexts.Aggregate(contexts.Length, (current, value) => current * 397 ^ value.GetHashCode());

        #endregion
    }
}
