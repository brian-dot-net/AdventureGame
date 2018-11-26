// <copyright file="TextConsoleTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Test
{
    using System;
    using System.IO;
    using System.Text;
    using FluentAssertions;
    using Xunit;

    public class TextConsoleTest
    {
        [Fact]
        public void RunNoInput()
        {
            TextConsole con = new TextConsole(new MessageBus(), TextReader.Null, TextWriter.Null);

            Action act = () => con.Run();

            act.Should().NotThrow();
        }

        [Fact]
        public void ReadsOneInputProducesOneOutput()
        {
            MessageBus bus = new MessageBus();
            StringBuilder output = new StringBuilder();
            StringWriter writer = new StringWriter(output);
            bus.Subscribe<InputMessage>(m => bus.Send(new OutputMessage($"I saw '{m.Line}'")));
            TextConsole con = new TextConsole(bus, new StringReader("one line"), writer);

            con.Run();

            output.ToString().Should().Be("I saw 'one line'\r\n");
        }
    }
}
