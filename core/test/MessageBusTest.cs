// <copyright file="MessageBusTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Test
{
    using System;
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
    }
}
