// <copyright file="ItemsTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Test
{
    using System;
    using System.Collections.Generic;
    using Adventure.Messages;
    using FluentAssertions;
    using Xunit;

    public sealed class ItemsTest
    {
        [Fact]
        public void AddOneItem()
        {
            MessageBus bus = new MessageBus();
            Items items = new Items(bus);

            Action act = () => items.Add("key", new TestItem(bus));

            act.Should().NotThrow();
        }

        [Fact]
        public void AddTwoItems()
        {
            MessageBus bus = new MessageBus();
            Items items = new Items(bus);

            items.Add("key", new TestItem(bus));
            Action act = () => items.Add("coin", new TestItem(bus));

            act.Should().NotThrow();
        }

        [Fact]
        public void RemoveOneOfTwoItems()
        {
            MessageBus bus = new MessageBus();
            Items items = new Items(bus);
            items.Add("key", new TestItem(bus));
            TestItem coin = new TestItem(bus);
            items.Add("coin", coin);

            Item taken = items.Remove("coin");

            taken.Should().BeSameAs(coin);
        }

        [Fact]
        public void RemoveTwoItems()
        {
            MessageBus bus = new MessageBus();
            Items items = new Items(bus);
            TestItem key = new TestItem(bus);
            items.Add("key", key);
            TestItem coin = new TestItem(bus);
            items.Add("coin", coin);

            Item takenCoin = items.Remove("coin");
            Item takenKey = items.Remove("key");

            takenCoin.Should().BeSameAs(coin);
            takenKey.Should().BeSameAs(key);
        }

        [Fact]
        public void RemoveItemAlreadyRemoved()
        {
            MessageBus bus = new MessageBus();
            Items items = new Items(bus);
            items.Add("key", new TestItem(bus));

            items.Remove("key");
            Item missing = items.Remove("key");

            missing.Should().BeNull();
        }

        [Fact]
        public void RemoveItemNotPresent()
        {
            Items items = new Items(new MessageBus());

            Item missing = items.Remove("key");

            missing.Should().BeNull();
        }

        [Fact]
        public void AddItemAlreadyExists()
        {
            MessageBus bus = new MessageBus();
            Items items = new Items(bus);
            items.Add("key", new TestItem(bus));

            Action act = () => items.Add("key", new TestItem(bus));

            act.Should().Throw<InvalidOperationException>("Item 'key' already exists.");
        }

        [Fact]
        public void DoCustomActionForItem()
        {
            MessageBus bus = new MessageBus();
            List<string> messages = new List<string>();
            bus.Subscribe<OutputMessage>(m => messages.Add(m.Text));
            Items items = new Items(bus);
            items.Add("ball", new TestItem(bus));

            items.Activate();
            bus.Subscribe<SentenceMessage>(m => messages.Add($"Don't {m.Verb} the {m.Noun}"));
            bus.Send(new SentenceMessage(new Word("throw", "TOSS"), new Word("ball", "BASEBALL")));

            messages.Should().ContainSingle().Which.Should().Be("You threw the BASEBALL!");
        }

        [Fact]
        public void SkipCustomActionForItemNotPresent()
        {
            MessageBus bus = new MessageBus();
            List<string> messages = new List<string>();
            bus.Subscribe<OutputMessage>(m => messages.Add(m.Text));
            Items items = new Items(bus);
            items.Add("ball", new TestItem(bus));

            items.Activate();
            bus.Subscribe<SentenceMessage>(m => messages.Add($"Don't {m.Verb} the {m.Noun}"));
            bus.Send(new SentenceMessage(new Word("throw", "TOSS"), new Word("party", "PARTY")));

            messages.Should().ContainSingle().Which.Should().Be("Don't TOSS the PARTY");
        }

        [Fact]
        public void SkipCustomActionForItemThatCannotHandleIt()
        {
            MessageBus bus = new MessageBus();
            List<string> messages = new List<string>();
            bus.Subscribe<OutputMessage>(m => messages.Add(m.Text));
            Items items = new Items(bus);
            items.Add("ball", new TestItem(bus));

            items.Activate();
            bus.Subscribe<SentenceMessage>(m => messages.Add($"Don't {m.Verb} the {m.Noun}"));
            bus.Send(new SentenceMessage(new Word("eat", "CONSUME"), new Word("ball", "BALL")));

            messages.Should().ContainSingle().Which.Should().Be("Don't CONSUME the BALL");
        }

        [Fact]
        public void SkipCustomActionForItemThatHasNoActions()
        {
            MessageBus bus = new MessageBus();
            List<string> messages = new List<string>();
            bus.Subscribe<OutputMessage>(m => messages.Add(m.Text));
            Items items = new Items(bus);
            items.Add("ball", new TestItemNoActions(bus));

            items.Activate();
            bus.Subscribe<SentenceMessage>(m => messages.Add($"Don't {m.Verb} the {m.Noun}"));
            bus.Send(new SentenceMessage(new Word("eat", "CONSUME"), new Word("ball", "BALL")));

            messages.Should().ContainSingle().Which.Should().Be("Don't CONSUME the BALL");
        }

        [Fact]
        public void SkipCustomActionBeforeActivation()
        {
            MessageBus bus = new MessageBus();
            List<string> messages = new List<string>();
            bus.Subscribe<OutputMessage>(m => messages.Add(m.Text));
            Items items = new Items(bus);
            items.Add("ball", new TestItem(bus));

            bus.Subscribe<SentenceMessage>(m => messages.Add($"Don't {m.Verb} the {m.Noun}"));
            bus.Send(new SentenceMessage(new Word("throw", "TOSS"), new Word("ball", "BASEBALL")));

            messages.Should().ContainSingle().Which.Should().Be("Don't TOSS the BASEBALL");
        }

        [Fact]
        public void SkipCustomActionAfterDeactivation()
        {
            MessageBus bus = new MessageBus();
            List<string> messages = new List<string>();
            bus.Subscribe<OutputMessage>(m => messages.Add(m.Text));
            Items items = new Items(bus);
            items.Add("ball", new TestItem(bus));

            items.Activate();
            items.Deactivate();
            bus.Subscribe<SentenceMessage>(m => messages.Add($"Don't {m.Verb} the {m.Noun}"));
            bus.Send(new SentenceMessage(new Word("throw", "TOSS"), new Word("ball", "BASEBALL")));

            messages.Should().ContainSingle().Which.Should().Be("Don't TOSS the BASEBALL");
        }

        [Fact]
        public void ActivateTwice()
        {
            MessageBus bus = new MessageBus();
            Items items = new Items(bus);

            items.Activate();
            Action act = () => items.Activate();

            act.Should().Throw<InvalidOperationException>().WithMessage("Cannot Activate again.");
        }

        [Fact]
        public void DeactivateTwice()
        {
            MessageBus bus = new MessageBus();
            Items items = new Items(bus);

            items.Activate();
            items.Deactivate();
            Action act = () => items.Deactivate();

            act.Should().Throw<InvalidOperationException>().WithMessage("Cannot Deactivate before Activate.");
        }

        [Fact]
        public void DeactivateBeforeActivate()
        {
            MessageBus bus = new MessageBus();
            Items items = new Items(bus);

            Action act = () => items.Deactivate();

            act.Should().Throw<InvalidOperationException>().WithMessage("Cannot Deactivate before Activate.");
        }

        [Fact]
        public void LookCountsItems()
        {
            MessageBus bus = new MessageBus();
            Items items = new Items(bus);
            items.Activate();
            items.Add("one", new TestItem(bus));
            items.Add("two", new TestItem(bus));

            int count = items.Look("{0}");

            count.Should().Be(2);
        }

        private sealed class TestItemNoActions : Item
        {
            public TestItemNoActions(MessageBus bus)
                : base(bus)
            {
            }

            public override string ShortDescription => "a dull item";

            public override string LongDescription => "It's a very dull item.";
        }

        private sealed class TestItem : Item
        {
            public TestItem(MessageBus bus)
                : base(bus)
            {
            }

            public override string ShortDescription => "a test item";

            public override string LongDescription => "It's a simple test item.";

            protected override bool DoCore(Word verb, Word noun)
            {
                if (verb.Primary == "throw")
                {
                    this.Output($"You threw the {noun}!");
                    return true;
                }

                return false;
            }
        }
    }
}
