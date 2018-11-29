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
            string[] parts = line.Trim().Split(new char[] { ' ' }, 2);
            string verb = parts[0];
            string noun = string.Empty;
            if (parts.Length == 2)
            {
                noun = parts[1].Trim();
            }

            this.bus.Send(new SentenceMessage(verb, noun));
        }
    }
}
