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
        public void ProcessTake()
        {
            TestSend(
                new Word("take", "GET"),
                new Word(string.Empty, string.Empty),
                "What do you want to GET?");
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
                "I don't know what 'BYE' means.");
        }

        [Fact]
        public void DropOneItem()
        {
            MessageBus bus = new MessageBus();
            List<string> output = new List<string>();
            Action<OutputMessage> subscriber = m => output.Add(m.Text);
            bus.Subscribe(subscriber);
            TestRoom room = new TestRoom(bus);
            room.TestDropItem("key", new TestKey());

            room.Enter();

            output.Should().Equal(
                "You are in a test room.",
                "There is a key here.");
        }

        [Fact]
        public void DropTwoItems()
        {
            MessageBus bus = new MessageBus();
            List<string> output = new List<string>();
            Action<OutputMessage> subscriber = m => output.Add(m.Text);
            bus.Subscribe(subscriber);
            TestRoom room = new TestRoom(bus);
            room.TestDropItem("key", new TestKey());
            room.TestDropItem("coin", new TestCoin());

            room.Enter();

            output.Should().Equal(
                "You are in a test room.",
                "There is a key here.",
                "There is a coin here.");
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
            public override string ShortDescription => "a key";
        }

        private sealed class TestCoin : Item
        {
            public override string ShortDescription => "a coin";
        }
    }
}
