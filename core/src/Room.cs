// <copyright file="Room.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    using System;
    using System.Collections.Generic;
    using Adventure.Messages;

    public abstract class Room
    {
        private readonly MessageBus bus;
        private readonly Dictionary<string, Action<Word, Word>> verbs;
        private readonly Items items;

        private IDisposable process;
        private IDisposable look;
        private IDisposable act;

        protected Room(MessageBus bus)
        {
            this.bus = bus;
            this.verbs = new Dictionary<string, Action<Word, Word>>(StringComparer.OrdinalIgnoreCase);
            this.items = new Items(this.bus);
        }

        protected abstract string Description { get; }

        public void Enter()
        {
            if (this.process != null)
            {
                throw new InvalidOperationException("Cannot Enter again.");
            }

            this.items.Activate();
            this.process = this.bus.Subscribe<SentenceMessage>(m => this.Process(m));
            this.look = this.bus.Subscribe<LookItemMessage>(m => this.LookAt(m.Noun));
            this.act = this.bus.Subscribe<RoomActionMessage>(m => m.Act(this));
            this.EnterCore();
            this.Look(new Word(string.Empty, string.Empty));
        }

        public void Leave()
        {
            if (this.process == null)
            {
                throw new InvalidOperationException("Cannot Leave before Enter.");
            }

            this.items.Deactivate();
            this.verbs.Clear();
            this.process.Dispose();
            this.process = null;
            this.look.Dispose();
            this.act.Dispose();
        }

        public void Add(string name, Item item)
        {
            this.items.Add(name, item);
        }

        public void End(string text = null)
        {
            this.bus.Send(new EndOfGameMessage(text));
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

        protected void Output(string text)
        {
            this.bus.Output(text);
        }

        protected void Inventory()
        {
            this.bus.Send(new ShowInventoryMessage());
        }

        protected void Look(Word noun)
        {
            if (noun.Actual.Length == 0)
            {
                this.LookAround();
            }
            else
            {
                this.bus.Send(new LookItemMessage(noun));
            }
        }

        protected virtual bool LookAtCore(Word noun)
        {
            return false;
        }

        protected void Drop(Word verb, Word noun)
        {
            if (noun.Actual.Length == 0)
            {
                this.Output($"What do you want to {verb}?");
            }
            else
            {
                this.bus.Send(new DropItemMessage(this.items, verb, noun));
            }
        }

        protected void Go(Word verb, Word noun)
        {
            if (noun.Actual.Length == 0)
            {
                this.Output($"Where do you want to {verb}?");
            }
            else if (!this.GoCore(noun))
            {
                this.bus.Send(new GoMessage(noun.Primary));
            }
        }

        protected void Take(Word verb, Word noun)
        {
            if (noun.Actual.Length == 0)
            {
                this.Output($"What do you want to {verb}?");
            }
            else if (!this.TakeCore(noun))
            {
                this.TakeItem(verb, noun);
            }
        }

        protected virtual bool TakeCore(Word noun)
        {
            return false;
        }

        protected virtual bool GoCore(Word noun)
        {
            return false;
        }

        private void TakeItem(Word verb, Word noun)
        {
            Item taken = this.items.Take(noun.Primary);
            if (taken == null)
            {
                this.Output($"You can't {verb} that.");
                return;
            }

            if (!taken.Take())
            {
                this.Add(noun.Primary, taken);
                return;
            }

            this.bus.Send(new TakeItemMessage(verb, noun, taken));
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

        private void LookAt(Word noun)
        {
            if (!this.items.LookAt(noun) && !this.LookAtCore(noun))
            {
                this.Output("You see nothing of interest.");
            }
        }
    }
}
