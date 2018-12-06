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

        [Fact]
        public void RunReadTwoLines()
        {
            MessageBus bus = new MessageBus();
            int linesRead = 0;
            bus.Subscribe<InputRequestedMessage>(_ =>
            {
                if (++linesRead == 2)
                {
                    bus.Send(new InputEndedMessage());
                }
            });
            using (InputLoop loop = new InputLoop(bus))
            {
                loop.Run(CancellationToken.None);

                linesRead.Should().Be(2);
            }
        }

        [Fact]
        public void RunCancelImmediately()
        {
            MessageBus bus = new MessageBus();
            bus.Subscribe<InputRequestedMessage>(_ => throw new InvalidOperationException("Should not have requested input."));
            using (CancellationTokenSource cts = new CancellationTokenSource())
            {
                cts.Cancel();
                using (InputLoop loop = new InputLoop(bus))
                {
                    Action act = () => loop.Run(cts.Token);

                    act.Should().NotThrow();
                }
            }
        }
    }
}
