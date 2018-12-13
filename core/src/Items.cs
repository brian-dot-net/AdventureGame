// <copyright file="Items.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class Items
    {
        private readonly MessageBus bus;
        private readonly Dictionary<string, Item> items;

        private IDisposable sub;

        public Items(MessageBus bus)
        {
            this.bus = bus;
            this.items = new Dictionary<string, Item>();
        }

        public void Activate()
        {
            if (this.sub != null)
            {
                throw new InvalidOperationException("Cannot Activate again.");
            }

            this.sub = this.bus.Subscribe<SentenceMessage>(m => this.Do(m));
        }

        public void Deactivate()
        {
            if (this.sub == null)
            {
                throw new InvalidOperationException("Cannot Deactivate before Activate.");
            }

            this.sub.Dispose();
            this.sub = null;
        }

        public int Look(string itemFormat)
        {
            int count = 0;
            foreach (string item in this.items.Values.Select(i => i.ShortDescription))
            {
                this.Output(string.Format(itemFormat, item));
                ++count;
            }

            return count;
        }

        public bool LookAt(Word noun)
        {
            if (this.items.TryGetValue(noun.Primary, out Item item))
            {
                this.Output(item.LongDescription);
                return true;
            }

            return false;
        }

        public void Drop(string name, Item item)
        {
            if (this.items.ContainsKey(name))
            {
                throw new InvalidOperationException($"Item '{name}' already exists.");
            }

            this.items.Add(name, item);
        }

        public Item Take(string name)
        {
            this.items.Remove(name, out Item item);
            return item;
        }

        private void Output(string text)
        {
            this.bus.Send(new OutputMessage(text));
        }

        private bool Do(SentenceMessage sentence)
        {
            if (this.items.TryGetValue(sentence.Noun.Primary, out Item item))
            {
                return item.Do(this.bus, sentence.Verb, sentence.Noun);
            }

            return false;
        }
    }
}
