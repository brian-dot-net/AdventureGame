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
            this.verbs = new Dictionary<string, Action<Word, Word>>(StringComparer.OrdinalIgnoreCase);
        }

        protected abstract string Description { get; }

        public void Enter()
        {
            if (this.sub != null)
            {
                throw new InvalidOperationException("Cannot Enter again.");
            }

            this.sub = this.bus.Subscribe<SentenceMessage>(m => this.Process(m));
            this.EnterCore();
            this.Look(new Word(string.Empty, string.Empty));
        }

        public void Leave()
        {
            if (this.sub == null)
            {
                throw new InvalidOperationException("Cannot Leave before Enter.");
            }

            this.verbs.Clear();
            this.sub.Dispose();
            this.sub = null;
        }

        protected virtual void EnterCore()
        {
        }

        protected void Register(string verb, Action<Word, Word> handler)
        {
            if (this.verbs.ContainsKey(verb))
            {
                throw new InvalidOperationException($"The verb '{verb}' is already registered.");
            }

            this.verbs.Add(verb, handler);
        }

        protected void Output(string message)
        {
            this.bus.Send(new OutputMessage(message));
        }

        protected void Look(Word noun)
        {
            if (noun.Actual.Length == 0)
            {
                this.Output(this.Description);
            }
            else if (!this.LookAt(noun))
            {
                this.Output($"I can't see any {noun} here.");
            }
        }

        protected virtual bool LookAt(Word noun)
        {
            return false;
        }

        private void Process(SentenceMessage message)
        {
            if (!this.verbs.TryGetValue(message.Verb.Primary, out Action<Word, Word> handler))
            {
                handler = this.UnknownVerb;
            }

            handler(message.Verb, message.Noun);
        }

        private void UnknownVerb(Word verb, Word noun)
        {
            this.Output($"I don't know what '{verb}' means.");
        }
    }
}
