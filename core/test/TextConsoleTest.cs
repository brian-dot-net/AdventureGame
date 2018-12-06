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
            using (OldInputLoop loop = con.NewLoop())
            {
                Action act = () => loop.Run(CancellationToken.None);

                act.Should().NotThrow();
            }
        }

        [Fact]
        public void ReadsOneInputProducesOneOutput()
        {
            MessageBus bus = new MessageBus();
            StringBuilder output = new StringBuilder();
            StringWriter writer = new StringWriter(output);
            bus.Subscribe<InputReceivedMessage>(m => bus.Send(new OutputMessage($"I saw '{m.Line}'")));
            TextConsole con = new TextConsole(bus, new StringReader("one line"), writer);
            using (OldInputLoop loop = con.NewLoop())
            {
                loop.Run(CancellationToken.None);

                output.ToString().Should().Be("I saw 'one line'\r\n");
            }
        }

        [Fact]
        public void ProducesNoOutputAfterDispose()
        {
            MessageBus bus = new MessageBus();
            StringBuilder output = new StringBuilder();
            StringWriter writer = new StringWriter(output);
            TextConsole con = new TextConsole(bus, TextReader.Null, writer);
            using (OldInputLoop loop = con.NewLoop())
            {
                loop.Run(CancellationToken.None);
            }

            bus.Send(new OutputMessage("do not print this"));

            output.ToString().Should().BeEmpty();
        }

        [Fact]
        public void BreaksOutOfLoopAfterCancellation()
        {
            using (CancellationTokenSource cts = new CancellationTokenSource())
            {
                MessageBus bus = new MessageBus();
                int calls = 0;
                Action<InputReceivedMessage> subscriber = m =>
                {
                    ++calls;
                    if (m.Line == "cancel")
                    {
                        cts.Cancel();
                    }
                };
                bus.Subscribe(subscriber);
                string[] lines = new string[] { "start", "cancel", "too late" };
                TextConsole con = new TextConsole(bus, new StringReader(string.Join(Environment.NewLine, lines)), TextWriter.Null);
                using (OldInputLoop loop = con.NewLoop())
                {
                    loop.Run(cts.Token);
                }

                calls.Should().Be(2);
            }
        }
    }
}
