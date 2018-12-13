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
        private readonly Items items;

        private IDisposable sub;

        protected Room(MessageBus bus)
        {
            this.bus = bus;
            this.verbs = new Dictionary<string, Action<Word, Word>>(StringComparer.OrdinalIgnoreCase);
            this.items = new Items(this.bus);
        }

        protected abstract string Description { get; }

        public void Enter()
        {
            if (this.sub != null)
            {
                throw new InvalidOperationException("Cannot Enter again.");
            }

            this.items.Activate();
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

            this.items.Deactivate();
            this.verbs.Clear();
            this.sub.Dispose();
            this.sub = null;
        }

        public void Drop(string name, Item item)
        {
            this.items.Drop(name, item);
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

        protected void Inventory()
        {
            this.bus.Send(new InventoryRequestedMessage());
        }

        protected void Look(Word noun)
        {
            if (noun.Actual.Length == 0)
            {
                this.LookAround();
            }
            else if (!this.LookAt(noun))
            {
                this.Output("You see nothing of interest.");
            }
        }

        protected virtual bool LookAtCore(Word noun)
        {
            return false;
        }

        protected void Take(Word verb, Word noun)
        {
            if (noun.Actual.Length == 0)
            {
                this.Output($"What do you want to {verb}?");
            }
            else if (!this.Take(noun))
            {
                this.Output($"You can't {verb} that.");
            }
        }

        protected virtual bool Take(Word noun)
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
            this.Output("You can't do that.");
        }

        private void LookAround()
        {
            this.Output(this.Description);
            this.items.Look("There is {0} here.");
        }

        private bool LookAt(Word noun)
        {
            return this.items.LookAt(noun) || this.LookAtCore(noun);
        }
    }
}
