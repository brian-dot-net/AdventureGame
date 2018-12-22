// <copyright file="ItemTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Test
{
    using System;
    using System.Collections.Generic;
    using Adventure.Messages;
    using FluentAssertions;
    using Xunit;

    public sealed class ItemTest
    {
        [Fact]
        public void NotTakenInitially()
        {
            MessageBus bus = new MessageBus();
            List<string> output = new List<string>();
            bus.Subscribe<OutputMessage>(m => output.Add(m.Text));
            Item item = new TestItem(bus);

            item.Do(new Word("use", "USE"), new Word("item", "ITEM"));

            output.Should().ContainSingle().Which.Should().Be("You must take it to use it.");
        }

        [Fact]
        public void Taken()
        {
            MessageBus bus = new MessageBus();
            List<string> output = new List<string>();
            bus.Subscribe<OutputMessage>(m => output.Add(m.Text));
            Item item = new TestItem(bus);

            item.Take().Should().BeTrue();
            item.Do(new Word("use", "USE"), new Word("item", "ITEM"));

            output.Should().ContainSingle().Which.Should().Be("How useful!");
        }

        private sealed class TestItem : Item
        {
            public TestItem(MessageBus bus)
                : base(bus)
            {
            }

            public override string ShortDescription => throw new NotImplementedException();

            public override string LongDescription => throw new NotImplementedException();

            protected override bool DoCore(Word verb, Word noun)
            {
                if (verb.Primary == "use")
                {
                    return this.Use();
                }

                return base.DoCore(verb, noun);
            }

            private bool Use()
            {
                if (!this.Taken)
                {
                    this.Output("You must take it to use it.");
                    return false;
                }

                this.Output("How useful!");
                return true;
            }
        }
    }
}
