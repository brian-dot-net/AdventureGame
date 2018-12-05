// <copyright file="QuitHandlerTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Test
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
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
    }
}
