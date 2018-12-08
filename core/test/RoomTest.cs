// <copyright file="RoomTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Test
{
    using System;
    using System.Collections.Generic;
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
            MessageBus bus = new MessageBus();
            string lastOutput = null;
            Action<OutputMessage> subscriber = m => lastOutput = m.Text;
            bus.Subscribe(subscriber);
            Room room = new TestRoom(bus);

            room.Enter();
            bus.Send(new SentenceMessage(new Word("look", "VIEW"), new Word(string.Empty, string.Empty)));

            lastOutput.Should().Be("You are in a test room.");
        }

        [Fact]
        public void ProcessLookUnknown()
        {
            MessageBus bus = new MessageBus();
            string lastOutput = null;
            Action<OutputMessage> subscriber = m => lastOutput = m.Text;
            bus.Subscribe(subscriber);
            Room room = new TestRoom(bus);

            room.Enter();
            bus.Send(new SentenceMessage(new Word("look", "VIEW"), new Word(string.Empty, "THING")));

            lastOutput.Should().Be("I can't see any THING here.");
        }

        [Fact]
        public void ProcessUnknownVerb()
        {
            MessageBus bus = new MessageBus();
            string lastOutput = null;
            Action<OutputMessage> subscriber = m => lastOutput = m.Text;
            bus.Subscribe(subscriber);
            Room room = new TestRoom(bus);

            room.Enter();
            bus.Send(new SentenceMessage(new Word("goodbye", "BYE"), new Word("world", "world")));

            lastOutput.Should().Be("I don't know what 'BYE' means.");
        }

        private sealed class TestRoom : Room
        {
            public TestRoom(MessageBus bus)
                : base(bus)
            {
            }

            protected override string Description => "You are in a test room.";

            public void TestRegisterHello(string verb)
            {
                this.Register(verb, this.Hello);
            }

            protected override void EnterCore()
            {
                this.TestRegisterHello("hello");
                this.Register("look", (_, n) => this.Look(n));
            }

            private void Hello(Word verb, Word noun)
            {
                this.Output("Hello, " + noun + "!");
            }
        }
    }
}
