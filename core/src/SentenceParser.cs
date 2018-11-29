// <copyright file="SentenceParser.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    public sealed class SentenceParser
    {
        private readonly MessageBus bus;

        public SentenceParser(MessageBus bus)
        {
            this.bus = bus;
            this.bus.Subscribe<InputMessage>(m => this.ProcessInput(m.Line));
        }

        private void ProcessInput(string line)
        {
            string[] parts = line.Split(new char[] { ' ' }, 2);
            if (parts.Length == 1)
            {
                this.bus.Send(new SentenceMessage(parts[0], string.Empty));
            }
            else
            {
                this.bus.Send(new SentenceMessage(parts[0], parts[1].Trim()));
            }
        }
    }
}
