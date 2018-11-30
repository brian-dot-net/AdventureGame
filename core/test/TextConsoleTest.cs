// <copyright file="TextConsoleTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Test
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading;
    using FluentAssertions;
    using Xunit;

    public class TextConsoleTest
    {
        [Fact]
        public void RunNoInput()
        {
            TextConsole con = new TextConsole(new MessageBus(), TextReader.Null, TextWriter.Null);

            Action act = () => con.Run(CancellationToken.None);

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

            con.Run(CancellationToken.None);

            output.ToString().Should().Be("I saw 'one line'\r\n");
        }

        [Fact]
        public void ProducesNoOutputAfterRun()
        {
            MessageBus bus = new MessageBus();
            StringBuilder output = new StringBuilder();
            StringWriter writer = new StringWriter(output);
            TextConsole con = new TextConsole(bus, TextReader.Null, writer);

            con.Run(CancellationToken.None);
            bus.Send(new OutputMessage("do not print this"));

            output.ToString().Should().BeEmpty();
        }
    }
}
