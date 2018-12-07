// <copyright file="TextConsoleTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Test
{
    using System.IO;
    using FluentAssertions;
    using Xunit;

    public sealed class TextConsoleTest
    {
        [Fact]
        public void ReadInputEnded()
        {
            MessageBus bus = new MessageBus();
            using (TextConsole console = new TextConsole(bus, TextReader.Null))
            {
                bool done = false;
                bus.Subscribe<InputEndedMessage>(m => done = true);

                bus.Send(new InputRequestedMessage());

                done.Should().BeTrue();
            }
        }
    }
}
