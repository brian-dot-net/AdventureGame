// <copyright file="Room.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    using System;
    using System.Collections.Generic;

    public abstract class Room
    {
        private readonly MessageBus bus;
        private readonly Dictionary<string, Action<Word, Word>> verbs;

        private IDisposable sub;

        protected Room(MessageBus bus)
        {
            this.bus = bus;
            this.verbs = new Dictionary<string, Action<Word, Word>>();
        }

        public void Enter()
        {
            this.sub = this.bus.Subscribe<SentenceMessage>(this.Process);
            this.EnterCore();
        }

        public void Leave()
        {
            this.sub.Dispose();
        }

        protected virtual void EnterCore()
        {
        }

        protected void Register(string verb, Action<Word, Word> handler)
        {
            this.verbs.Add(verb, handler);
        }

        protected void Output(string message)
        {
            this.bus.Send(new OutputMessage(message));
        }

        private void Process(SentenceMessage message)
        {
            this.verbs[message.Verb.Primary](message.Verb, message.Noun);
        }
    }
}
