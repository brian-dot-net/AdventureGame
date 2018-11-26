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

        public TextConsole(MessageBus bus, TextReader reader, TextWriter writer)
        {
            this.bus = bus;
            this.reader = reader;

            this.bus.Subscribe<OutputMessage>(m => writer.WriteLine(m.Text));
        }

        public void Run()
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
            while (line != null);
        }
    }
}
