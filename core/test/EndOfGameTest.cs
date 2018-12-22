// <copyright file="EndOfGameTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Test
{
    using Adventure.Messages;
    using FluentAssertions;
    using Xunit;

    public class EndOfGameTest
    {
        [Fact]
        public void EndOfGameSendsCancellation()
        {
            MessageBus bus = new MessageBus();
            string lastOutput = null;
            bus.Subscribe<OutputMessage>(m => lastOutput = "[" + m.Text + "]");
            using (EndOfGame end = new EndOfGame(bus))
            {
                end.Token.IsCancellationRequested.Should().BeFalse();

                bus.Send(new EndOfGameMessage("It's over."));

                end.Token.IsCancellationRequested.Should().BeTrue();
                lastOutput.Should().Be("[It's over.]");
            }
        }

        [Fact]
        public void EndOfGameNoMessageSendsCancellationNoOutput()
        {
            MessageBus bus = new MessageBus();
            string lastOutput = null;
            bus.Subscribe<OutputMessage>(m => lastOutput = "[" + m.Text + "]");
            using (EndOfGame end = new EndOfGame(bus))
            {
                end.Token.IsCancellationRequested.Should().BeFalse();

                bus.Send(new EndOfGameMessage());

                end.Token.IsCancellationRequested.Should().BeTrue();
                lastOutput.Should().BeNull();
            }
        }
    }
}
