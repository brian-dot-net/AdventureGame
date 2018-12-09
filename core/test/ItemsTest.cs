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
        public void AddOneItem()
        {
            Items items = new Items();

            Action act = () => items.Add("key", new TestItem());

            act.Should().NotThrow();
        }

        [Fact]
        public void AddTwoItems()
        {
            Items items = new Items();

            items.Add("key", new TestItem());
            Action act = () => items.Add("coin", new TestItem());

            act.Should().NotThrow();
        }

        [Fact]
        public void TakeOneOfTwoItems()
        {
            Items items = new Items();
            items.Add("key", new TestItem());
            TestItem coin = new TestItem();
            items.Add("coin", coin);

            Item taken = items.Take("coin");

            taken.Should().BeSameAs(coin);
        }

        [Fact]
        public void TakeTwoItems()
        {
            Items items = new Items();
            TestItem key = new TestItem();
            items.Add("key", key);
            TestItem coin = new TestItem();
            items.Add("coin", coin);

            Item takenCoin = items.Take("coin");
            Item takenKey = items.Take("key");

            takenCoin.Should().BeSameAs(coin);
            takenKey.Should().BeSameAs(key);
        }

        [Fact]
        public void TakeItemAlreadyTaken()
        {
            Items items = new Items();
            items.Add("key", new TestItem());

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

        private sealed class TestItem : Item
        {
        }
    }
}
