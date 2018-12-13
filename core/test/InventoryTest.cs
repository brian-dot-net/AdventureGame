// <copyright file="InventoryTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Test
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using Xunit;

    public sealed class InventoryTest
    {
        [Fact]
        public void ShowInventoryEmpty()
        {
            MessageBus bus = new MessageBus();
            List<string> messages = new List<string>();
            bus.Subscribe<OutputMessage>(m => messages.Add(m.Text));
            using (Inventory inv = new Inventory(bus))
            {
                bus.Send(new InventoryRequestedMessage());

                messages.Should().Equal("You are carrying:", "(nothing)");
            }
        }

        [Fact]
        public void ShowInventoryOneItem()
        {
            MessageBus bus = new MessageBus();
            List<string> messages = new List<string>();
            bus.Subscribe<OutputMessage>(m => messages.Add(m.Text));
            using (Inventory inv = new Inventory(bus))
            {
                inv.Drop("key", new TestItem());
                bus.Send(new InventoryRequestedMessage());

                messages.Should().Equal("You are carrying:", "a key");
            }
        }

        [Fact]
        public void ShowInventoryTwoItems()
        {
            MessageBus bus = new MessageBus();
            List<string> messages = new List<string>();
            bus.Subscribe<OutputMessage>(m => messages.Add(m.Text));
            using (Inventory inv = new Inventory(bus))
            {
                inv.Drop("key", new TestItem());
                inv.Drop("coin", new TestItem2());
                bus.Send(new InventoryRequestedMessage());

                messages.Should().Equal("You are carrying:", "a key", "a coin");
            }
        }

        [Fact]
        public void ShowInventoryAfterDispose()
        {
            MessageBus bus = new MessageBus();
            List<string> messages = new List<string>();
            bus.Subscribe<OutputMessage>(m => messages.Add(m.Text));
            using (Inventory inv = new Inventory(bus))
            {
                inv.Drop("key", new TestItem());
                inv.Drop("coin", new TestItem2());
            }

            bus.Send(new InventoryRequestedMessage());

            messages.Should().BeEmpty();
        }

        [Fact]
        public void ProcessCustomItemAction()
        {
            MessageBus bus = new MessageBus();
            string lastOutput = null;
            Action<OutputMessage> subscriber = m => lastOutput = m.Text;
            bus.Subscribe(subscriber);
            using (Inventory inv = new Inventory(bus))
            {
                inv.Drop("key", new TestItem());
                inv.Drop("coin", new TestItem2());

                bus.Send(new SentenceMessage(new Word("flip", "FLIP"), new Word("coin", "COIN")));

                lastOutput.Should().Be("You FLIP the COIN; it lands on heads.");
            }
        }

        [Fact]
        public void ProcessCustomItemActionAfterDispose()
        {
            MessageBus bus = new MessageBus();
            string lastOutput = null;
            Action<OutputMessage> subscriber = m => lastOutput = m.Text;
            bus.Subscribe(subscriber);
            using (Inventory inv = new Inventory(bus))
            {
                inv.Drop("key", new TestItem());
                inv.Drop("coin", new TestItem2());
            }

            bus.Send(new SentenceMessage(new Word("flip", "FLIP"), new Word("coin", "COIN")));

            lastOutput.Should().BeNull();
        }

        [Fact]
        public void AddInventory()
        {
            MessageBus bus = new MessageBus();
            List<string> messages = new List<string>();
            bus.Subscribe<OutputMessage>(m => messages.Add(m.Text));
            using (Inventory inv = new Inventory(bus))
            {
                bus.Send(new InventoryAddedMessage(new Word("take", "GRAB"), new Word("key", "KEY"), new TestItem()));
                bus.Send(new InventoryRequestedMessage());

                messages.Should().Equal("You GRAB the KEY.", "You are carrying:", "a key");
            }
        }

        private sealed class TestItem : Item
        {
            public override string ShortDescription => "a key";

            public override string LongDescription => throw new System.NotImplementedException();
        }

        private sealed class TestItem2 : Item
        {
            public override string ShortDescription => "a coin";

            public override string LongDescription => throw new System.NotImplementedException();

            protected override bool DoCore(MessageBus bus, Word verb, Word noun)
            {
                if (verb.Primary == "flip")
                {
                    bus.Send(new OutputMessage($"You {verb} the {noun}; it lands on heads."));
                    return true;
                }

                return false;
            }
        }
    }
}
