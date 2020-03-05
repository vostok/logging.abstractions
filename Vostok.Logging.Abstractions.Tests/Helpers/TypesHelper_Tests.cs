using System;
using FluentAssertions;
using NUnit.Framework;
using Vostok.Logging.Abstractions.Helpers;

namespace Vostok.Logging.Abstractions.Tests.Helpers
{
    [TestFixture]
    internal class TypesHelper_Tests
    {
        [Test]
        public void IsConstructedGenericType_should_be_true_for_constructed_type()
        {
            var obj = new
            {
                A = 42
            };

            TypesHelper.IsConstructedGenericType(obj.GetType()).Should().BeTrue();
        }

        [Test]
        public void IsConstructedGenericType_should_be_false_for_non_constructed_type()
        {
            var obj = new MyClass();

            TypesHelper.IsConstructedGenericType(obj.GetType()).Should().BeFalse();
            TypesHelper.IsConstructedGenericType(typeof(MyClass)).Should().BeFalse();
        }

        [Test]
        public void IsConstructedGenericType_should_be_false_for_primitive_type()
        {
            var obj = "hello";

            TypesHelper.IsConstructedGenericType(obj.GetType()).Should().BeFalse();
            TypesHelper.IsConstructedGenericType(typeof(string)).Should().BeFalse();
            TypesHelper.IsConstructedGenericType(typeof(int)).Should().BeFalse();
        }

        [Test]
        public void IsConstructedGenericType_should_be_false_for_nullable_type()
        {
            Guid? obj = Guid.NewGuid();

            TypesHelper.IsConstructedGenericType(obj.GetType()).Should().BeFalse();
            TypesHelper.IsConstructedGenericType(typeof(Guid?)).Should().BeFalse();
        }

        internal class MyClass
        {
            
        }
    }
}