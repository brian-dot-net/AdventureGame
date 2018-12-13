// <copyright file="InventoryTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Test
{
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

        private sealed class TestItem : Item
        {
            public override string ShortDescription => "a key";

            public override string LongDescription => throw new System.NotImplementedException();
        }

        private sealed class TestItem2 : Item
        {
            public override string ShortDescription => "a coin";

            public override string LongDescription => throw new System.NotImplementedException();
        }
    }
}
