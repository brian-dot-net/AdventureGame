// <copyright file="TextConsole.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    using System;
    using System.IO;
    using Adventure.Messages;

    public sealed class TextConsole : IDisposable
    {
        private readonly MessageBus bus;
        private readonly TextReader reader;
        private readonly TextWriter writer;
        private readonly IDisposable inputSub;
        private readonly IDisposable outputSub;

        public TextConsole(MessageBus bus, TextReader reader, TextWriter writer)
        {
            this.bus = bus;
            this.reader = reader;
            this.writer = writer;
            this.inputSub = this.bus.Subscribe<InputRequestedMessage>(m => this.ReadLine(m.Prompt));
            this.outputSub = this.bus.Subscribe<OutputMessage>(m => this.writer.WriteLine(m.Text));
        }

        public void Dispose()
        {
            this.inputSub.Dispose();
            this.outputSub.Dispose();
        }

        private void ReadLine(string prompt)
        {
            if (prompt != null)
            {
                this.writer.Write(prompt + " ");
            }

            string line = this.reader.ReadLine();
            if (line == null)
            {
                this.bus.Send(new InputEndedMessage());
            }
            else
            {
                this.bus.Send(new InputReceivedMessage(line));
            }
        }
    }
}
