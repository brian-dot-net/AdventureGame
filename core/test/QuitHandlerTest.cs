// <copyright file="QuitHandlerTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Test
{
    using System;
    using Adventure.Messages;
    using FluentAssertions;
    using Xunit;

    public class QuitHandlerTest
    {
        [Fact]
        public void QuitSendsCancellation()
        {
            MessageBus bus = new MessageBus();
            using (QuitHandler handler = new QuitHandler(bus, "scram"))
            {
                handler.Token.IsCancellationRequested.Should().BeFalse();

                bus.Send(new SentenceMessage(new Word("scram", "quit"), new Word(string.Empty, string.Empty)));

                handler.Token.IsCancellationRequested.Should().BeTrue();
            }
        }

        [Fact]
        public void DoesNothingAfterDispose()
        {
            bool secondSubscriber = false;
            MessageBus bus = new MessageBus();
            using (QuitHandler handler = new QuitHandler(bus, "scram"))
            {
                bus.Subscribe<SentenceMessage>(m => secondSubscriber = true);
            }

            Action act = () => bus.Send(new SentenceMessage(new Word("scram", "quit"), new Word(string.Empty, string.Empty)));

            act.Should().NotThrow();
            secondSubscriber.Should().BeTrue();
        }
    }
}
