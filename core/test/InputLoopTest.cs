// <copyright file="InputLoopTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Test
{
    using System;
    using System.Threading;
    using FluentAssertions;
    using Xunit;

    public class InputLoopTest
    {
        [Fact]
        public void RunInputEnded()
        {
            MessageBus bus = new MessageBus();
            bus.Subscribe<InputRequestedMessage>(_ => bus.Send(new InputEndedMessage()));
            using (InputLoop loop = new InputLoop(bus))
            {
                Action act = () => loop.Run(CancellationToken.None);

                act.Should().NotThrow();
            }
        }
    }
}
