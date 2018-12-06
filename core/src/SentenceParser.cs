// <copyright file="SentenceParser.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    using System;

    public sealed class SentenceParser : IDisposable
    {
        private readonly MessageBus bus;
        private readonly Words words;
        private readonly IDisposable subscription;

        public SentenceParser(MessageBus bus, Words words)
        {
            this.bus = bus;
            this.words = words;
            this.subscription = this.bus.Subscribe<InputReceivedMessage>(m => this.ProcessInput(m.Line));
        }

        public void Dispose()
        {
            this.subscription.Dispose();
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

            this.bus.Send(new SentenceMessage(this.words[verb], this.words[noun]));
        }
    }
}
