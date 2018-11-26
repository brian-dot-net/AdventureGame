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
    }
}
