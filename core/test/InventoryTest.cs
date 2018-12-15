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
                inv.Add("key", new TestItem());
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
                inv.Add("key", new TestItem());
                inv.Add("coin", new TestItem2());
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
                inv.Add("key", new TestItem());
                inv.Add("coin", new TestItem2());
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
                inv.Add("key", new TestItem());
                inv.Add("coin", new TestItem2());

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
                inv.Add("key", new TestItem());
                inv.Add("coin", new TestItem2());
            }

            bus.Send(new SentenceMessage(new Word("flip", "FLIP"), new Word("coin", "COIN")));

            lastOutput.Should().BeNull();
        }

        [Fact]
        public void ProcessLookItem()
        {
            MessageBus bus = new MessageBus();
            string lastOutput = null;
            bus.Subscribe<OutputMessage>(m => lastOutput = m.Text);
            using (Inventory inv = new Inventory(bus))
            {
                bus.Subscribe<LookItemMessage>(m => bus.Send(new OutputMessage($"I can't bear to look at {m.Noun}.")));
                inv.Add("key", new TestItem());
                inv.Add("coin", new TestItem2());

                bus.Send(new LookItemMessage(new Word("key", "KEY")));

                lastOutput.Should().Be("It's a test key.");
            }
        }

        [Fact]
        public void ProcessLookUnknown()
        {
            MessageBus bus = new MessageBus();
            string lastOutput = null;
            bus.Subscribe<OutputMessage>(m => lastOutput = m.Text);
            using (Inventory inv = new Inventory(bus))
            {
                bus.Subscribe<LookItemMessage>(m => bus.Send(new OutputMessage($"I can't bear to look at {m.Noun}.")));
                inv.Add("key", new TestItem());
                inv.Add("coin", new TestItem2());

                bus.Send(new LookItemMessage(new Word("that", "THAT")));

                lastOutput.Should().Be("I can't bear to look at THAT.");
            }
        }

        [Fact]
        public void ProcessLookAfterDispose()
        {
            MessageBus bus = new MessageBus();
            string lastOutput = null;
            bus.Subscribe<OutputMessage>(m => lastOutput = m.Text);
            using (Inventory inv = new Inventory(bus))
            {
                bus.Subscribe<LookItemMessage>(m => bus.Send(new OutputMessage($"I can't bear to look at {m.Noun}.")));
                inv.Add("key", new TestItem());
                inv.Add("coin", new TestItem2());
            }

            bus.Send(new LookItemMessage(new Word("key", "KEY")));

            lastOutput.Should().Be("I can't bear to look at KEY.");
        }

        [Fact]
        public void DropAllowedItem()
        {
            MessageBus bus = new MessageBus();
            List<string> messages = new List<string>();
            bus.Subscribe<OutputMessage>(m => messages.Add(m.Text));
            Items items = new Items(bus);
            using (Inventory inv = new Inventory(bus))
            {
                inv.Add("key", new TestItem());
                bus.Send(new InventoryDropMessage(items, new Word("drop", "THROW"), new Word("key", "KEY")));
                bus.Send(new InventoryRequestedMessage());

                messages.Should().Equal("You THROW the KEY.", "You are carrying:", "(nothing)");
                items.Look("{0}").Should().Be(1);
            }
        }

        [Fact]
        public void DropDisallowedItem()
        {
            MessageBus bus = new MessageBus();
            List<string> messages = new List<string>();
            bus.Subscribe<OutputMessage>(m => messages.Add(m.Text));
            Items items = new Items(bus);
            using (Inventory inv = new Inventory(bus))
            {
                inv.Add("key", new TestItem(false));
                bus.Send(new InventoryDropMessage(items, new Word("drop", "THROW"), new Word("key", "KEY")));
                bus.Send(new InventoryRequestedMessage());

                messages.Should().Equal("I won't let you drop this!", "You are carrying:", "a key");
                items.Look("{0}").Should().Be(0);
            }
        }

        [Fact]
        public void DropMissingItem()
        {
            MessageBus bus = new MessageBus();
            List<string> messages = new List<string>();
            bus.Subscribe<OutputMessage>(m => messages.Add(m.Text));
            Items items = new Items(bus);
            using (Inventory inv = new Inventory(bus))
            {
                bus.Send(new InventoryDropMessage(items, new Word("drop", "THROW"), new Word("key", "KEY")));

                messages.Should().Equal("You can't THROW what you don't have.");
                items.Look("{0}").Should().Be(0);
            }
        }

        [Fact]
        public void DropItemAfterDispose()
        {
            MessageBus bus = new MessageBus();
            List<string> messages = new List<string>();
            bus.Subscribe<OutputMessage>(m => messages.Add(m.Text));
            Items items = new Items(bus);
            using (Inventory inv = new Inventory(bus))
            {
                inv.Add("key", new TestItem());
            }

            bus.Send(new InventoryDropMessage(items, new Word("drop", "THROW"), new Word("key", "KEY")));

            messages.Should().BeEmpty();
            items.Look("{0}").Should().Be(0);
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

        [Fact]
        public void AddInventoryAfterDispose()
        {
            MessageBus bus = new MessageBus();
            List<string> messages = new List<string>();
            bus.Subscribe<OutputMessage>(m => messages.Add(m.Text));
            Inventory inv = new Inventory(bus);
            inv.Dispose();
            bus.Send(new InventoryAddedMessage(new Word("take", "GRAB"), new Word("key", "KEY"), new TestItem()));

            messages.Should().BeEmpty();
        }

        private sealed class TestItem : Item
        {
            private readonly bool canDrop;

            public TestItem(bool canDrop = true)
            {
                this.canDrop = canDrop;
            }

            public override string ShortDescription => "a key";

            public override string LongDescription => "It's a test key.";

            protected override bool DropCore(MessageBus bus)
            {
                if (!this.canDrop)
                {
                    bus.Send(new OutputMessage("I won't let you drop this!"));
                    return false;
                }

                return base.DropCore(bus);
            }
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
