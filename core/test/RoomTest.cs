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
            bus.Send(new SentenceMessage(new Word("hello", "hello"), new Word("world", "world")));

            output.Should().ContainSingle().Which.Should().Be("Hello, world!");
        }

        [Fact]
        public void UnsubscribeOnLeave()
        {
            MessageBus bus = new MessageBus();
            List<string> output = new List<string>();
            Action<OutputMessage> subscriber = m => output.Add(m.Text);
            bus.Subscribe(subscriber);
            Room room = new TestRoom(bus);

            room.Enter();
            room.Leave();
            bus.Send(new SentenceMessage(new Word("hello", "hello"), new Word("world", "world")));

            output.Should().BeEmpty();
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

        private sealed class TestRoom : Room
        {
            public TestRoom(MessageBus bus)
                : base(bus)
            {
            }

            protected override void EnterCore()
            {
                this.Register("hello", this.Hello);
            }

            private void Hello(Word verb, Word noun)
            {
                this.Output("Hello, " + noun + "!");
            }
        }
    }
}
