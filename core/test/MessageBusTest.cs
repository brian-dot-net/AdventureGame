// <copyright file="MessageBusTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Test
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using Xunit;

    public class MessageBusTest
    {
        [Fact]
        public void SendStringNoSubscribers()
        {
            MessageBus bus = new MessageBus();

            Action act = () => bus.Send("hello");

            act.Should().NotThrow();
        }

        [Fact]
        public void SendStringOneSubscriber()
        {
            List<string> received = new List<string>();
            MessageBus bus = new MessageBus();
            Action<string> subscriber = m => received.Add("S1=" + m);

            bus.Subscribe(subscriber);
            bus.Send("hello");

            received.Should().Equal("S1=hello");
        }

        [Fact]
        public void SendStringTwoSubscribers()
        {
            List<string> received = new List<string>();
            MessageBus bus = new MessageBus();
            Action<string> subscriber1 = m => received.Add("S1=" + m);
            Action<string> subscriber2 = m => received.Add("S2=" + m);

            bus.Subscribe(subscriber1);
            bus.Subscribe(subscriber2);
            bus.Send("hello");

            received.Should().Equal("S1=hello", "S2=hello");
        }

        [Fact]
        public void SendIntAndStringOneSubscriberEach()
        {
            List<string> received = new List<string>();
            MessageBus bus = new MessageBus();
            Action<string> subscriber1 = m => received.Add("S1=" + m);
            Action<int> subscriber2 = m => received.Add("S2=" + m);

            bus.Subscribe(subscriber1);
            bus.Subscribe(subscriber2);
            bus.Send("hello");
            bus.Send(123);

            received.Should().Equal("S1=hello", "S2=123");
        }

        [Fact]
        public void OneSubscriberUnsubscribeBeforeSend()
        {
            List<string> received = new List<string>();
            MessageBus bus = new MessageBus();
            Action<string> subscriber = m => received.Add("S1=" + m);

            using (bus.Subscribe(subscriber))
            {
            }

            bus.Send("hello");

            received.Should().BeEmpty();
        }

        [Fact]
        public void TwoSubscribersUnsubscribeOneBeforeSend()
        {
            List<string> received = new List<string>();
            MessageBus bus = new MessageBus();
            Action<string> subscriber1 = m => received.Add("S1=" + m);
            Action<string> subscriber2 = m => received.Add("S2=" + m);

            using (bus.Subscribe(subscriber1))
            {
                bus.Subscribe(subscriber2);
            }

            bus.Send("hello");

            received.Should().Equal("S2=hello");
        }

        [Fact]
        public void TwoSubscribersUnsubscribeOneDuringSend()
        {
            List<string> received = new List<string>();
            MessageBus bus = new MessageBus();
            IDisposable sub1 = null;
            Action<string> subscriber1 = _ => sub1.Dispose();
            Action<string> subscriber2 = m => received.Add("S2=" + m);

            sub1 = bus.Subscribe(subscriber1);
            bus.Subscribe(subscriber2);

            bus.Send("hello");

            received.Should().Equal("S2=hello");
        }

        [Fact]
        public void OneSubscriberThrowsOnReceive()
        {
            MessageBus bus = new MessageBus();
            Action<string> subscriber = _ => throw new InvalidProgramException("whoops");

            bus.Subscribe(subscriber);
            Action act = () => bus.Send("hello");

            act.Should().Throw<InvalidProgramException>().WithMessage("whoops");
        }

        [Fact]
        public void TwoSubscribersFirstThrowsOnReceive()
        {
            List<string> received = new List<string>();
            MessageBus bus = new MessageBus();
            Action<string> subscriber1 = _ => throw new InvalidProgramException("whoops");
            Action<string> subscriber2 = m => received.Add("S2=" + m);

            bus.Subscribe(subscriber1);
            bus.Subscribe(subscriber2);
            Action act = () => bus.Send("hello");

            act.Should().Throw<InvalidProgramException>().WithMessage("whoops");
            received.Should().BeEmpty();
        }

        [Fact]
        public void TwoSubscribersSecondThrowsOnReceive()
        {
            List<string> received = new List<string>();
            MessageBus bus = new MessageBus();
            Action<string> subscriber1 = m => received.Add("S1=" + m);
            Action<string> subscriber2 = _ => throw new InvalidProgramException("whoops");

            bus.Subscribe(subscriber1);
            bus.Subscribe(subscriber2);
            Action act = () => bus.Send("hello");

            act.Should().Throw<InvalidProgramException>().WithMessage("whoops");
            received.Should().Equal("S1=hello");
        }

        [Fact]
        public void ThreeSubscribersUnsubscribeSecond()
        {
            List<string> received = new List<string>();
            MessageBus bus = new MessageBus();
            Action<string> subscriber1 = m => received.Add("S1=" + m);
            Action<string> subscriber2 = m => received.Add("S2=" + m);
            Action<string> subscriber3 = m => received.Add("S3=" + m);

            bus.Subscribe(subscriber1);
            bus.Subscribe(subscriber2).Dispose();
            bus.Subscribe(subscriber3);
            bus.Send("hello");

            received.Should().Equal("S1=hello", "S3=hello");
        }

        [Fact]
        public void SendStringOneBoolSubscriber()
        {
            bool received = false;
            MessageBus bus = new MessageBus();
            Func<string, bool> subscriber = m => received = bool.Parse(m);

            bus.Subscribe(subscriber);
            bus.Send("true");

            received.Should().BeTrue();
        }

        [Fact]
        public void SendStringTwoBoolSubscribersFirstReturnsFalse()
        {
            int charCount = 0;
            MessageBus bus = new MessageBus();
            Func<string, bool> subscriber1 = m => (charCount += m.Length) == 0;
            Func<string, bool> subscriber2 = m => (charCount += m.Length) == 6;

            bus.Subscribe(subscriber1);
            bus.Subscribe(subscriber2);
            bus.Send("abc");

            charCount.Should().Be(6);
        }

        [Fact]
        public void SendStringTwoBoolSubscribersFirstReturnsTrue()
        {
            int charCount = 0;
            MessageBus bus = new MessageBus();
            Func<string, bool> subscriber1 = m => (charCount += m.Length) == 3;
            Func<string, bool> subscriber2 = m => (charCount += m.Length) == 6;

            bus.Subscribe(subscriber1);
            bus.Subscribe(subscriber2);
            bus.Send("abc");

            charCount.Should().Be(3);
        }
    }
}
