// <copyright file="TextConsole.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    using System.IO;

    public sealed class TextConsole
    {
        private readonly MessageBus bus;
        private readonly TextReader reader;
        private readonly TextWriter writer;

        public TextConsole(MessageBus bus, TextReader reader, TextWriter writer)
        {
            this.bus = bus;
            this.reader = reader;
            this.writer = writer;
        }

        public InputLoop NewLoop()
        {
            return new InputLoop(this.bus, this.ReadLine, m => this.writer.WriteLine(m.Text));
        }

        private bool ReadLine()
        {
            string line = this.reader.ReadLine();
            if (line == null)
            {
                return false;
            }

            this.bus.Send(new InputMessage(line));
            return true;
        }
    }
}
