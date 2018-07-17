using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using Snapshot = Vostok.Logging.Abstractions.Helpers.DictionarySnapshot<string, string>;

// ReSharper disable ReturnValueOfPureMethodIsNotUsed

namespace Vostok.Logging.Abstractions.Tests
{
    [TestFixture]
    internal class DictionarySnapshot_Tests
    {
        [Test]
        public void Empty_instance_should_not_contain_any_values()
        {
            Snapshot.Empty.Should().BeEmpty();
        }

        [Test]
        public void Empty_instance_should_have_zero_count()
        {
            Snapshot.Empty.Count.Should().Be(0);
        }

        [Test]
        public void Should_be_able_to_add_a_value_to_empty_instance()
        {
            var snapshot = Snapshot.Empty.Set("k", "v");

            snapshot.Count.Should().Be(1);
            snapshot["k"].Should().Be("v");
        }

        [Test]
        public void Should_not_modify_empty_instance_when_deriving_from_it()
        {
            Snapshot.Empty.Set("k", "v");

            Snapshot.Empty.Should().BeEmpty();

            Snapshot.Empty.Count.Should().Be(0);
        }

        [Test]
        public void Should_use_provided_equality_comparer_when_comparing_keys()
        {
            var snapshot = new Snapshot(1, StringComparer.OrdinalIgnoreCase)
                .Set("key", "value")
                .Set("KEY", "value");

            snapshot.Should().HaveCount(1);

            snapshot.Keys.Should().Equal("key");
        }

        [Test]
        public void Set_should_be_able_to_expand_beyond_initial_capacity()
        {
            var snapshot = new Snapshot(1)
                .Set("k1", "v1")
                .Set("k2", "v2")
                .Set("k3", "v3")
                .Set("k4", "v4")
                .Set("k5", "v5");

            snapshot.Count.Should().Be(5);

            snapshot.Keys.Should().Equal("k1", "k2", "k3", "k4", "k5");
        }

        [Test]
        public void Set_should_return_same_instance_when_replacing_a_value_with_same_value()
        {
            var snapshotBefore = Snapshot.Empty
                .Set("k1", "v1")
                .Set("k2", "v2")
                .Set("k3", "v3");

            var snapshotAfter = snapshotBefore.Set("k2", "v2");

            snapshotAfter.Should().BeSameAs(snapshotBefore);
        }

        [Test]
        public void Set_should_replace_existing_value_when_provided_with_a_different_value()
        {
            var snapshotBefore = Snapshot.Empty
                .Set("k1", "v1")
                .Set("k2", "v2")
                .Set("k3", "v3");

            var snapshotAfter = snapshotBefore.Set("k2", "vx");

            snapshotAfter.Should().NotBeSameAs(snapshotBefore);
            snapshotAfter.Count.Should().Be(3);
            snapshotAfter["k1"].Should().Be("v1");
            snapshotAfter["k2"].Should().Be("vx");
            snapshotAfter["k3"].Should().Be("v3");
        }

        [Test]
        public void Set_should_not_overwrite_existing_value_when_it_is_explicitly_prohibited()
        {
            var snapshotBefore = Snapshot.Empty
                .Set("k1", "v1")
                .Set("k2", "v2")
                .Set("k3", "v3");

            var snapshotAfter = snapshotBefore.Set("k2", "vx", false);

            snapshotAfter.Should().BeSameAs(snapshotBefore);
        }

        [Test]
        public void Set_should_not_modify_base_instance_when_deriving_from_it_by_replacing_existing_value()
        {
            var snapshotBefore = Snapshot.Empty
                .Set("k1", "v1")
                .Set("k2", "v2")
                .Set("k3", "v3");

            var snapshotAfter = snapshotBefore.Set("k2", "vx");

            snapshotAfter.Should().NotBeSameAs(snapshotBefore);
            snapshotBefore.Count.Should().Be(3);
            snapshotBefore["k1"].Should().Be("v1");
            snapshotBefore["k2"].Should().Be("v2");
            snapshotBefore["k3"].Should().Be("v3");
        }

        [Test]
        public void Set_should_be_able_to_grow_new_instance_when_adding_unique_keys_without_spoiling_base_instance()
        {
            var snapshot = new Snapshot(0);

            for (var i = 0; i < 100; i++)
            {
                var newKey = "key-" + i;
                var newValue = "value-" + i;
                var newSnapshot = snapshot.Set(newKey, newValue);

                newSnapshot.Should().NotBeSameAs(snapshot);
                newSnapshot.Count.Should().Be(i + 1);
                newSnapshot[newKey].Should().Be(newValue);

                snapshot.Count.Should().Be(i);
                snapshot.ContainsKey(newKey).Should().BeFalse();
                snapshot = newSnapshot;
            }
        }

        [Test]
        public void Set_should_correctly_handle_forking_multiple_instances_from_a_single_base()
        {
            var snapshotBefore = Snapshot.Empty
                .Set("k1", "v1")
                .Set("k2", "v2")
                .Set("k3", "v3");

            var snapshotAfter1 = snapshotBefore.Set("k4", "v4-1");
            var snapshotAfter2 = snapshotBefore.Set("k4", "v4-2");

            snapshotBefore.Count.Should().Be(3);
            snapshotBefore.ContainsKey("k4").Should().BeFalse();

            snapshotAfter1.Should().NotBeSameAs(snapshotBefore);
            snapshotAfter1.Count.Should().Be(4);
            snapshotAfter1["k4"].Should().Be("v4-1");

            snapshotAfter2.Should().NotBeSameAs(snapshotBefore);
            snapshotAfter2.Should().NotBeSameAs(snapshotAfter1);
            snapshotAfter2.Count.Should().Be(4);
            snapshotAfter2["k4"].Should().Be("v4-2");
        }

        [Test]
        public void Remove_should_correctly_remove_first_value()
        {
            var snapshotBefore = Snapshot.Empty
                .Set("k1", "v1")
                .Set("k2", "v2")
                .Set("k3", "v3")
                .Set("k4", "v4")
                .Set("k5", "v5");

            var snapshotAfter = snapshotBefore.Remove("k1");

            snapshotAfter.Should().NotBeSameAs(snapshotBefore);
            snapshotAfter.Should().HaveCount(4);
            snapshotAfter.ContainsKey("k1").Should().BeFalse();
        }

        [Test]
        public void Remove_should_correctly_remove_last_value()
        {
            var snapshotBefore = Snapshot.Empty
                .Set("k1", "v1")
                .Set("k2", "v2")
                .Set("k3", "v3")
                .Set("k4", "v4")
                .Set("k5", "v5");

            var snapshotAfter = snapshotBefore.Remove("k5");

            snapshotAfter.Should().NotBeSameAs(snapshotBefore);
            snapshotAfter.Should().HaveCount(4);
            snapshotAfter.ContainsKey("k5").Should().BeFalse();
        }

        [Test]
        public void Remove_should_correctly_remove_a_value_from_the_middle()
        {
            var snapshotBefore = Snapshot.Empty
                .Set("k1", "v1")
                .Set("k2", "v2")
                .Set("k3", "v3")
                .Set("k4", "v4")
                .Set("k5", "v5");

            var snapshotAfter = snapshotBefore.Remove("k3");

            snapshotAfter.Should().NotBeSameAs(snapshotBefore);
            snapshotAfter.Should().HaveCount(4);
            snapshotAfter.ContainsKey("k3").Should().BeFalse();
        }

        [Test]
        public void Remove_should_correctly_remove_the_only_value_in_collection()
        {
            var snapshotBefore = Snapshot.Empty
                .Set("k1", "v1");

            var snapshotAfter = snapshotBefore.Remove("k1");

            snapshotAfter.Should().NotBeSameAs(snapshotBefore);
            snapshotAfter.Count.Should().Be(0);
            snapshotAfter.Should().BeEmpty();
        }

        [Test]
        public void Remove_should_return_same_instance_when_removing_a_value_which_is_not_present()
        {
            var snapshotBefore = Snapshot.Empty
                .Set("k1", "v1")
                .Set("k2", "v2")
                .Set("k3", "v3")
                .Set("k4", "v4")
                .Set("k5", "v5");

            var snapshotAfter = snapshotBefore.Remove("k6");

            snapshotAfter.Should().BeSameAs(snapshotBefore);
        }

        [Test]
        public void Remove_should_not_spoil_base_insance()
        {
            var snapshotBefore = Snapshot.Empty
                .Set("k1", "v1")
                .Set("k2", "v2")
                .Set("k3", "v3")
                .Set("k4", "v4")
                .Set("k5", "v5");

            snapshotBefore.Remove("k1");
            snapshotBefore.Remove("k2");
            snapshotBefore.Remove("k3");
            snapshotBefore.Remove("k4");
            snapshotBefore.Remove("k5");

            snapshotBefore.Should().HaveCount(5);
            snapshotBefore["k1"].Should().Be("v1");
            snapshotBefore["k2"].Should().Be("v2");
            snapshotBefore["k3"].Should().Be("v3");
            snapshotBefore["k4"].Should().Be("v4");
            snapshotBefore["k5"].Should().Be("v5");
        }

        [Test]
        public void Indexer_should_throw_an_exception_when_snapshot_are_empty()
        {
            Action action = () => Snapshot.Empty["name"].GetHashCode();

            var error = action.Should().Throw<KeyNotFoundException>().Which;

            Console.Out.WriteLine(error);
        }

        [Test]
        public void Indexer_should_return_null_when_value_with_given_key_does_not_exist()
        {
            var snapshot = Snapshot.Empty
                .Set("key1", "value1")
                .Set("key2", "value2");

            Action action = () => snapshot["key3"].GetHashCode();

            var error = action.Should().Throw<KeyNotFoundException>().Which;

            Console.Out.WriteLine(error);
        }

        [Test]
        public void Indexer_should_return_correct_values_for_existing_keys()
        {
            var snapshot = Snapshot.Empty
                .Set("key1", "value1")
                .Set("key2", "value2");

            snapshot["key1"].Should().Be("value1");
            snapshot["key2"].Should().Be("value2");
        }

        [Test]
        public void Indexer_should_use_provided_key_comparer_when_comparing_keys()
        {
            var snapshot = new Snapshot(StringComparer.OrdinalIgnoreCase)
                .Set("name", "value1")
                .Set("NAME", "value2");

            snapshot["name"].Should().Be("value2");
        }
    }
}
