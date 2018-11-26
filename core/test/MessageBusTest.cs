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
    }
}
