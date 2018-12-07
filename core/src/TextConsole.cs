// <copyright file="TextConsole.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    using System;
    using System.IO;

    public sealed class TextConsole : IDisposable
    {
        private readonly MessageBus bus;
        private readonly IDisposable inputSub;
        private readonly IDisposable outputSub;

        public TextConsole(MessageBus bus, TextReader reader, TextWriter writer)
        {
            this.bus = bus;
            this.inputSub = this.bus.Subscribe<InputRequestedMessage>(_ => this.ReadLine(reader));
            this.outputSub = this.bus.Subscribe<OutputMessage>(m => writer.WriteLine(m.Text));
        }

        public void Dispose()
        {
            this.inputSub.Dispose();
            this.outputSub.Dispose();
        }

        private void ReadLine(TextReader reader)
        {
            string line = reader.ReadLine();
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
