// <copyright file="ItemsTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Test
{
    using System;
    using FluentAssertions;
    using Xunit;

    public sealed class ItemsTest
    {
        [Fact]
        public void DropOneItem()
        {
            Items items = new Items();

            Action act = () => items.Drop("key", new TestItem());

            act.Should().NotThrow();
        }

        [Fact]
        public void DropTwoItems()
        {
            Items items = new Items();

            items.Drop("key", new TestItem());
            Action act = () => items.Drop("coin", new TestItem());

            act.Should().NotThrow();
        }

        [Fact]
        public void TakeOneOfTwoItems()
        {
            Items items = new Items();
            items.Drop("key", new TestItem());
            TestItem coin = new TestItem();
            items.Drop("coin", coin);

            Item taken = items.Take("coin");

            taken.Should().BeSameAs(coin);
        }

        [Fact]
        public void TakeTwoItems()
        {
            Items items = new Items();
            TestItem key = new TestItem();
            items.Drop("key", key);
            TestItem coin = new TestItem();
            items.Drop("coin", coin);

            Item takenCoin = items.Take("coin");
            Item takenKey = items.Take("key");

            takenCoin.Should().BeSameAs(coin);
            takenKey.Should().BeSameAs(key);
        }

        [Fact]
        public void TakeItemAlreadyTaken()
        {
            Items items = new Items();
            items.Drop("key", new TestItem());

            items.Take("key");
            Item missing = items.Take("key");

            missing.Should().BeNull();
        }

        [Fact]
        public void TakeItemNotPresent()
        {
            Items items = new Items();

            Item missing = items.Take("key");

            missing.Should().BeNull();
        }

        [Fact]
        public void DropItemAlreadyExists()
        {
            Items items = new Items();
            items.Drop("key", new TestItem());

            Action act = () => items.Drop("key", new TestItem());

            act.Should().Throw<InvalidOperationException>("Item 'key' already exists.");
        }

        private sealed class TestItem : Item
        {
            public override string ShortDescription => "a test item";
        }
    }
}
