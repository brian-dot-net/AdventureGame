// <copyright file="ItemsTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Test
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using Xunit;

    public sealed class ItemsTest
    {
        [Fact]
        public void DropOneItem()
        {
            Items items = new Items(new MessageBus());

            Action act = () => items.Drop("key", new TestItem());

            act.Should().NotThrow();
        }

        [Fact]
        public void DropTwoItems()
        {
            Items items = new Items(new MessageBus());

            items.Drop("key", new TestItem());
            Action act = () => items.Drop("coin", new TestItem());

            act.Should().NotThrow();
        }

        [Fact]
        public void TakeOneOfTwoItems()
        {
            Items items = new Items(new MessageBus());
            items.Drop("key", new TestItem());
            TestItem coin = new TestItem();
            items.Drop("coin", coin);

            Item taken = items.Take("coin");

            taken.Should().BeSameAs(coin);
        }

        [Fact]
        public void TakeTwoItems()
        {
            Items items = new Items(new MessageBus());
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
            Items items = new Items(new MessageBus());
            items.Drop("key", new TestItem());

            items.Take("key");
            Item missing = items.Take("key");

            missing.Should().BeNull();
        }

        [Fact]
        public void TakeItemNotPresent()
        {
            Items items = new Items(new MessageBus());

            Item missing = items.Take("key");

            missing.Should().BeNull();
        }

        [Fact]
        public void DropItemAlreadyExists()
        {
            Items items = new Items(new MessageBus());
            items.Drop("key", new TestItem());

            Action act = () => items.Drop("key", new TestItem());

            act.Should().Throw<InvalidOperationException>("Item 'key' already exists.");
        }

        [Fact]
        public void DoCustomActionForItem()
        {
            MessageBus bus = new MessageBus();
            List<string> messages = new List<string>();
            bus.Subscribe<OutputMessage>(m => messages.Add(m.Text));
            Items items = new Items(bus);
            items.Drop("ball", new TestItem());

            items.Activate();
            bus.Send(new SentenceMessage(new Word("throw", "TOSS"), new Word("ball", "BASEBALL")));

            messages.Should().ContainSingle().Which.Should().Be("You threw the BASEBALL!");
        }

        private sealed class TestItem : Item
        {
            public override string ShortDescription => "a test item";

            protected override void DoCore(MessageBus bus, Word verb, Word noun)
            {
                if (verb.Primary == "throw")
                {
                    bus.Send(new OutputMessage($"You threw the {noun}!"));
                }
            }
        }
    }
}
