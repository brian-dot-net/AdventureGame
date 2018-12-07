// <copyright file="TextConsoleTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Test
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using FluentAssertions;
    using Xunit;

    public sealed class TextConsoleTest
    {
        [Fact]
        public void ReadInputEnded()
        {
            MessageBus bus = new MessageBus();
            using (TextConsole console = new TextConsole(bus, TextReader.Null, TextWriter.Null))
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
            using (TextConsole console = new TextConsole(bus, reader, TextWriter.Null))
            {
                List<string> lines = new List<string>();
                bus.Subscribe<InputReceivedMessage>(m => lines.Add(m.Line));

                bus.Send(new InputRequestedMessage());

                lines.Should().ContainSingle().Which.Should().Be("one");

                bus.Send(new InputRequestedMessage());

                lines.Should().Equal("one", "two");
            }
        }

        [Fact]
        public void ReadAfterDispose()
        {
            MessageBus bus = new MessageBus();
            List<string> lines = new List<string>();
            bus.Subscribe<InputReceivedMessage>(m => lines.Add(m.Line));
            using (StringReader reader = new StringReader("one" + Environment.NewLine + "two"))
            {
                using (TextConsole console = new TextConsole(bus, reader, TextWriter.Null))
                {
                }

                bus.Send(new InputRequestedMessage());

                lines.Should().BeEmpty();
            }
        }

        [Fact]
        public void WriteTwoLines()
        {
            MessageBus bus = new MessageBus();
            StringBuilder sb = new StringBuilder();
            using (StringWriter writer = new StringWriter(sb))
            using (TextConsole console = new TextConsole(bus, TextReader.Null, writer))
            {
                bus.Send(new OutputMessage("one"));

                sb.ToString().Should().Be("one" + Environment.NewLine);

                bus.Send(new OutputMessage("two"));

                sb.ToString().Should().Be("one" + Environment.NewLine + "two" + Environment.NewLine);
            }
        }
    }
}
