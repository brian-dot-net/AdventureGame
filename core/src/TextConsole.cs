// <copyright file="TextConsole.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    using System.IO;
    using System.Threading;

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

        public void Run(CancellationToken token)
        {
            using (this.bus.Subscribe<OutputMessage>(m => this.writer.WriteLine(m.Text)))
            {
                string line;
                do
                {
                    line = this.reader.ReadLine();
                    if (line != null)
                    {
                        this.bus.Send(new InputMessage(line));
                    }
                }
                while (!token.IsCancellationRequested && (line != null));
            }
        }
    }
}
