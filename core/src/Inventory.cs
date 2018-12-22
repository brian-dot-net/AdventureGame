// <copyright file="Inventory.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    using System;
    using Adventure.Messages;

    public sealed class Inventory : IDisposable
    {
        private readonly MessageBus bus;
        private readonly IDisposable show;
        private readonly IDisposable take;
        private readonly IDisposable drop;
        private readonly IDisposable look;
        private readonly Items items;

        public Inventory(MessageBus bus)
        {
            this.bus = bus;
            this.show = bus.Subscribe<ShowInventoryMessage>(m => this.Show());
            this.take = bus.Subscribe<TakeItemMessage>(m => this.Take(m.Verb, m.Noun, m.Item));
            this.drop = bus.Subscribe<DropItemMessage>(m => this.Drop(m.Verb, m.Noun, m.Items));
            this.look = bus.Subscribe<LookItemMessage>(m => this.Look(m.Noun));
            this.items = new Items(this.bus);
            this.items.Activate();
        }

        public void Dispose()
        {
            this.items.Deactivate();
            this.show.Dispose();
            this.take.Dispose();
            this.drop.Dispose();
            this.look.Dispose();
        }

        public void Add(string key, Item item)
        {
            this.items.Add(key, item);
        }

        public Item Remove(string key) => this.items.Remove(key);

        private void Show()
        {
            this.Output("You are carrying:");
            int count = this.items.Look("{0}");
            if (count == 0)
            {
                this.Output("(nothing)");
            }
        }

        private void Take(Word verb, Word noun, Item item)
        {
            this.Add(noun.Primary, item);
            this.Output($"You {verb} the {noun}.");
        }

        private void Drop(Word verb, Word noun, Items targetItems)
        {
            Item item = this.items.Remove(noun.Primary);
            if (item == null)
            {
                this.Output($"You can't {verb} what you don't have.");
                return;
            }

            if (item.Drop())
            {
                targetItems.Add(noun.Primary, item);
                this.Output($"You {verb} the {noun}.");
                return;
            }

            this.items.Add(noun.Primary, item);
        }

        private bool Look(Word noun)
        {
            return this.items.LookAt(noun);
        }

        private void Output(string text)
        {
            this.bus.Output(text);
        }
    }
}
