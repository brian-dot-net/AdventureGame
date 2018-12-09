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

        private sealed class TestItem : Item
        {
        }
    }
}
