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
            this.bus.Send(new SentenceMessage(line));
        }
    }
}
