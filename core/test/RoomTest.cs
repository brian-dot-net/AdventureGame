// <copyright file="RoomTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Test
{
    using System;
    using System.Collections.Generic;
    using Adventure.Messages;
    using FluentAssertions;
    using Xunit;

    public sealed class RoomTest
    {
        [Fact]
        public void RegisterOnEnter()
        {
            MessageBus bus = new MessageBus();
            List<string> output = new List<string>();
            Action<OutputMessage> subscriber = m => output.Add(m.Text);
            bus.Subscribe(subscriber);
            Room room = new TestRoom(bus);

            room.Enter();

            output.Should().ContainSingle().Which.Should().Be("You are in a test room.");

            bus.Send(new SentenceMessage(new Word("hello", "hello"), new Word("world", "world")));

            output.Should().Contain("Hello, world!");
        }

        [Fact]
        public void UnsubscribeOnLeave()
        {
            MessageBus bus = new MessageBus();
            Room room = new TestRoom(bus);
            string lastOutput = null;
            Action<OutputMessage> subscriber = m => lastOutput = m.Text;

            room.Enter();
            room.Leave();
            bus.Subscribe(subscriber);
            bus.Send(new SentenceMessage(new Word("hello", "hello"), new Word("world", "world")));

            lastOutput.Should().BeNull();
        }

        [Fact]
        public void LeaveBeforeEnter()
        {
            Room room = new TestRoom(new MessageBus());

            Action act = () => room.Leave();

            act.Should().Throw<InvalidOperationException>().WithMessage("Cannot Leave before Enter.");
        }

        [Fact]
        public void LeaveTwice()
        {
            Room room = new TestRoom(new MessageBus());
            room.Enter();

            room.Leave();
            Action act = () => room.Leave();

            act.Should().Throw<InvalidOperationException>().WithMessage("Cannot Leave before Enter.");
        }

        [Fact]
        public void EnterTwice()
        {
            Room room = new TestRoom(new MessageBus());
            room.Enter();

            Action act = () => room.Enter();

            act.Should().Throw<InvalidOperationException>().WithMessage("Cannot Enter again.");
        }

        [Fact]
        public void EnterLeaveEnter()
        {
            MessageBus bus = new MessageBus();
            string lastOutput = null;
            Action<OutputMessage> subscriber = m => lastOutput = m.Text;
            bus.Subscribe(subscriber);
            Room room = new TestRoom(bus);

            room.Enter();
            room.Leave();
            room.Enter();
            bus.Send(new SentenceMessage(new Word("hello", "hello"), new Word("world", "world")));

            lastOutput.Should().Be("Hello, world!");
        }

        [Fact]
        public void RegisterSameVerb()
        {
            MessageBus bus = new MessageBus();
            List<string> output = new List<string>();
            Action<OutputMessage> subscriber = m => output.Add(m.Text);
            bus.Subscribe(subscriber);
            TestRoom room = new TestRoom(bus);

            room.Enter();
            Action act = () => room.TestRegisterHello("hello");

            act.Should().Throw<InvalidOperationException>().WithMessage("The verb 'hello' is already registered.");
        }

        [Fact]
        public void RegisterSameVerbDifferentCase()
        {
            MessageBus bus = new MessageBus();
            List<string> output = new List<string>();
            Action<OutputMessage> subscriber = m => output.Add(m.Text);
            bus.Subscribe(subscriber);
            TestRoom room = new TestRoom(bus);

            room.Enter();
            Action act = () => room.TestRegisterHello("HeLLO");

            act.Should().Throw<InvalidOperationException>().WithMessage("The verb 'HeLLO' is already registered.");
        }

        [Fact]
        public void ProcessLook()
        {
            TestSend(
                new Word("look", "VIEW"),
                new Word(string.Empty, string.Empty),
                "You are in a test room.");
        }

        [Fact]
        public void ProcessLookCustom()
        {
            TestSend(
                new Word("look", "VIEW"),
                new Word("up", "SKY"),
                "You see the ceiling.");
        }

        [Fact]
        public void ProcessLookUnknown()
        {
            TestSend(
                new Word("look", "VIEW"),
                new Word(string.Empty, "THING"),
                "You see nothing of interest.");
        }

        [Fact]
        public void ProcessLookItem()
        {
            MessageBus bus = new MessageBus();
            string lastOutput = null;
            Action<OutputMessage> subscriber = m => lastOutput = m.Text;
            bus.Subscribe(subscriber);
            Room room = new TestRoom(bus);
            room.Add("key", new TestKey());

            room.Enter();
            bus.Send(new SentenceMessage(new Word("look", "LOOK"), new Word("key", "KEY")));

            lastOutput.Should().Be("It is solid gold.");
        }

        [Fact]
        public void ProcessLookItemUpperLevelSubscriber()
        {
            MessageBus bus = new MessageBus();
            string lastOutput = null;
            bus.Subscribe<OutputMessage>(m => lastOutput = m.Text);
            Dictionary<string, Item> items = new Dictionary<string, Item>();
            items.Add("key", new TestKey());
            bus.Subscribe<LookItemMessage>(m =>
            {
                bus.Send(new OutputMessage(items[m.Noun.Primary].LongDescription));
                return true;
            });
            Room room = new TestRoom(bus);

            room.Enter();
            bus.Send(new SentenceMessage(new Word("look", "LOOK"), new Word("key", "KEY")));

            lastOutput.Should().Be("It is solid gold.");
        }

        [Fact]
        public void ProcessLookItemAfterLeave()
        {
            MessageBus bus = new MessageBus();
            List<string> messages = new List<string>();
            bus.Subscribe<OutputMessage>(m => messages.Add(m.Text));
            Room room = new TestRoom(bus);

            room.Enter();
            room.Leave();
            bus.Send(new LookItemMessage(new Word("key", "KEY")));

            messages.Should().Equal("You are in a test room.");
        }

        [Fact]
        public void ProcessTake()
        {
            TestSend(
                new Word("take", "GET"),
                new Word(string.Empty, string.Empty),
                "What do you want to GET?");
        }

        [Fact]
        public void ProcessTakeAvailableItem()
        {
            MessageBus bus = new MessageBus();
            List<string> messages = new List<string>();
            bus.Subscribe<OutputMessage>(m => messages.Add(m.Text));
            Item actualItem = null;
            bus.Subscribe<TakeItemMessage>(m =>
            {
                messages.Add($"You {m.Verb} the {m.Noun}!");
                actualItem = m.Item;
            });
            TestRoom room = new TestRoom(bus);
            Item expectedItem = new TestKey();
            room.Add("key", expectedItem);
            room.Add("coin", new TestCoin());

            room.Enter();
            bus.Send(new SentenceMessage(new Word("take", "TAKE"), new Word("key", "KEY")));
            bus.Send(new SentenceMessage(new Word("look", "LOOK"), new Word(string.Empty, string.Empty)));

            messages.Should().Equal(
                "You are in a test room.",
                "There is a key here.",
                "There is a coin here.",
                "You TAKE the KEY!",
                "You are in a test room.",
                "There is a coin here.");
            actualItem.Should().BeSameAs(expectedItem);
        }

        [Fact]
        public void ProcessTakeUnavailableItem()
        {
            MessageBus bus = new MessageBus();
            List<string> messages = new List<string>();
            bus.Subscribe<OutputMessage>(m => messages.Add(m.Text));
            Item actualItem = null;
            bus.Subscribe<TakeItemMessage>(m =>
            {
                messages.Add($"You {m.Verb} the {m.Noun}!");
                actualItem = m.Item;
            });
            TestRoom room = new TestRoom(bus);
            room.Add("key", new TestKey(false));
            room.Add("coin", new TestCoin());

            room.Enter();
            bus.Send(new SentenceMessage(new Word("take", "TAKE"), new Word("key", "KEY")));
            bus.Send(new SentenceMessage(new Word("look", "LOOK"), new Word(string.Empty, string.Empty)));

            messages.Should().Equal(
                "You are in a test room.",
                "There is a key here.",
                "There is a coin here.",
                "I won't let you take this!",
                "You are in a test room.",
                "There is a key here.",
                "There is a coin here.");
            actualItem.Should().BeNull();
        }

        [Fact]
        public void ProcessTakeUnknown()
        {
            TestSend(
                new Word("take", "grab"),
                new Word(string.Empty, "THING"),
                "You can't grab that.");
        }

        [Fact]
        public void ProcessTakeCustom()
        {
            TestSend(
                new Word("take", "RETRIEVE"),
                new Word("breath", "breath"),
                "You inhale deeply.");
        }

        [Fact]
        public void ProcessUnknownVerb()
        {
            TestSend(
                new Word("goodbye", "BYE"),
                new Word("world", "world"),
                "You can't do that.");
        }

        [Fact]
        public void ProcessDrop()
        {
            TestSend(
                new Word("drop", "THROW"),
                new Word(string.Empty, string.Empty),
                "What do you want to THROW?");
        }

        [Fact]
        public void DropOneItem()
        {
            MessageBus bus = new MessageBus();
            Dictionary<string, Item> items = new Dictionary<string, Item>();
            items["DROP/KEY"] = new TestKey();
            bus.Subscribe<DropItemMessage>(m => m.Items.Add(m.Noun.Primary, items[$"{m.Verb}/{m.Noun}"]));
            List<string> output = new List<string>();
            bus.Subscribe<OutputMessage>(m => output.Add(m.Text));
            TestRoom room = new TestRoom(bus);

            room.Enter();
            bus.Send(new SentenceMessage(new Word("drop", "DROP"), new Word("key", "KEY")));
            bus.Send(new SentenceMessage(new Word("look", "LOOK"), new Word(string.Empty, string.Empty)));

            output.Should().Equal(
                "You are in a test room.",
                "You are in a test room.",
                "There is a key here.");
        }

        [Fact]
        public void AddOneItem()
        {
            MessageBus bus = new MessageBus();
            List<string> output = new List<string>();
            Action<OutputMessage> subscriber = m => output.Add(m.Text);
            bus.Subscribe(subscriber);
            TestRoom room = new TestRoom(bus);
            room.Add("key", new TestKey());

            room.Enter();

            output.Should().Equal(
                "You are in a test room.",
                "There is a key here.");
        }

        [Fact]
        public void AddTwoItems()
        {
            MessageBus bus = new MessageBus();
            List<string> output = new List<string>();
            Action<OutputMessage> subscriber = m => output.Add(m.Text);
            bus.Subscribe(subscriber);
            TestRoom room = new TestRoom(bus);
            room.Add("key", new TestKey());
            room.Add("coin", new TestCoin());

            room.Enter();

            output.Should().Equal(
                "You are in a test room.",
                "There is a key here.",
                "There is a coin here.");
        }

        [Fact]
        public void ProcessCustomItemAction()
        {
            MessageBus bus = new MessageBus();
            string lastOutput = null;
            Action<OutputMessage> subscriber = m => lastOutput = m.Text;
            bus.Subscribe(subscriber);
            TestRoom room = new TestRoom(bus);
            room.Add("key", new TestKey());
            room.Add("coin", new TestCoin());

            room.Enter();
            bus.Send(new SentenceMessage(new Word("flip", "FLIP"), new Word("coin", "COIN")));

            lastOutput.Should().Be("You FLIP the COIN; it lands on heads.");
        }

        [Fact]
        public void ProcessCustomItemActionAfterLeave()
        {
            MessageBus bus = new MessageBus();
            string lastOutput = null;
            Action<OutputMessage> subscriber = m => lastOutput = m.Text;
            bus.Subscribe(subscriber);
            TestRoom room = new TestRoom(bus);
            room.Add("key", new TestKey());
            room.Add("coin", new TestCoin());

            room.Enter();
            room.Leave();
            bus.Send(new SentenceMessage(new Word("flip", "FLIP"), new Word("coin", "COIN")));

            lastOutput.Should().Be("There is a coin here.");
        }

        [Fact]
        public void ProcessUnknownCustomItemAction()
        {
            MessageBus bus = new MessageBus();
            string lastOutput = null;
            Action<OutputMessage> subscriber = m => lastOutput = m.Text;
            bus.Subscribe(subscriber);
            TestRoom room = new TestRoom(bus);
            room.Add("key", new TestKey());
            room.Add("coin", new TestCoin());

            room.Enter();
            bus.Send(new SentenceMessage(new Word("flip", "FLIP"), new Word("key", "KEY")));

            lastOutput.Should().Be("You can't do that.");
        }

        [Fact]
        public void RequestInventory()
        {
            MessageBus bus = new MessageBus();
            int inv = 0;
            bus.Subscribe<ShowInventoryMessage>(_ => ++inv);
            TestRoom room = new TestRoom(bus);

            room.Enter();
            bus.Send(new SentenceMessage(new Word("inventory", "INV"), new Word(string.Empty, string.Empty)));

            inv.Should().Be(1);
        }

        private static void TestSend(Word verb, Word noun, string expectedOutput)
        {
            MessageBus bus = new MessageBus();
            string lastOutput = null;
            Action<OutputMessage> subscriber = m => lastOutput = m.Text;
            bus.Subscribe(subscriber);
            Room room = new TestRoom(bus);

            room.Enter();
            bus.Send(new SentenceMessage(verb, noun));

            lastOutput.Should().Be(expectedOutput);
        }

        private sealed class TestKey : Item
        {
            private readonly bool canTake;

            public TestKey(bool canTake = true)
            {
                this.canTake = canTake;
            }

            public override string ShortDescription => "a key";

            public override string LongDescription => "It is solid gold.";

            protected override bool TakeCore(MessageBus bus)
            {
                if (this.canTake)
                {
                    return base.TakeCore(bus);
                }

                bus.Send(new OutputMessage("I won't let you take this!"));
                return false;
            }
        }

        private sealed class TestCoin : Item
        {
            public override string ShortDescription => "a coin";

            public override string LongDescription => "It is a shiny new quarter.";

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
