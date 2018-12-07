// <copyright file="TextConsoleTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Test
{
    using System;
    using System.Collections.Generic;
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

        [Fact]
        public void ReadTwoLines()
        {
            MessageBus bus = new MessageBus();
            using (StringReader reader = new StringReader("one" + Environment.NewLine + "two"))
            using (TextConsole console = new TextConsole(bus, reader))
            {
                List<string> lines = new List<string>();
                bus.Subscribe<InputReceivedMessage>(m => lines.Add(m.Line));

                bus.Send(new InputRequestedMessage());

                lines.Should().ContainSingle().Which.Should().Be("one");

                bus.Send(new InputRequestedMessage());

                lines.Should().Equal("one", "two");
            }
        }
    }
}
