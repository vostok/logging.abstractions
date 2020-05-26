using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using Vostok.Logging.Abstractions.Helpers;

namespace Vostok.Logging.Abstractions.Tests.Helpers
{
    [TestFixture]
    internal class TypesHelper_Tests
    {
        [Test]
        public void IsAnonymousType_should_be_true_for_anonymous_type()
        {
            var obj = new
            {
                A = 42
            };

            TypesHelper.IsAnonymousType(obj.GetType()).Should().BeTrue();
        }

        [Test]
        public void IsAnonymousType_should_be_false_for_non_anonymous_type()
        {
            var obj = new MyClass();

            TypesHelper.IsAnonymousType(obj.GetType()).Should().BeFalse();
            TypesHelper.IsAnonymousType(typeof(MyClass)).Should().BeFalse();
        }

        [Test]
        public void IsAnonymousType_should_be_false_for_primitive_type()
        {
            var obj = "hello";

            TypesHelper.IsAnonymousType(obj.GetType()).Should().BeFalse();
            TypesHelper.IsAnonymousType(typeof(string)).Should().BeFalse();
            TypesHelper.IsAnonymousType(typeof(int)).Should().BeFalse();
        }

        [Test]
        public void IsAnonymousType_should_be_false_for_nullable_type()
        {
            Guid? obj = Guid.NewGuid();

            TypesHelper.IsAnonymousType(obj.GetType()).Should().BeFalse();
            TypesHelper.IsAnonymousType(typeof(Guid?)).Should().BeFalse();
        }

        [Test]
        public void IsAnonymousType_should_be_false_for_generic_collections()
        {
            TypesHelper.IsAnonymousType(typeof(List<int>)).Should().BeFalse();
            TypesHelper.IsAnonymousType(typeof(Dictionary<int, string>)).Should().BeFalse();
        }

        internal class MyClass
        {
            
        }
    }
}