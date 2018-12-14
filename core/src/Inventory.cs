// <copyright file="Inventory.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    using System;

    public sealed class Inventory : IDisposable
    {
        private readonly MessageBus bus;
        private readonly IDisposable show;
        private readonly IDisposable add;
        private readonly IDisposable drop;
        private readonly Items items;

        public Inventory(MessageBus bus)
        {
            this.bus = bus;
            this.show = bus.Subscribe<InventoryRequestedMessage>(m => this.Show());
            this.add = bus.Subscribe<InventoryAddedMessage>(m => this.Add(m.Verb, m.Noun, m.Item));
            this.drop = bus.Subscribe<InventoryDropMessage>(m => this.Drop(m.Verb, m.Noun, m.Items));
            this.items = new Items(this.bus);
            this.items.Activate();
        }

        public void Dispose()
        {
            this.items.Deactivate();
            this.show.Dispose();
            this.add.Dispose();
            this.drop.Dispose();
        }

        public void Add(string key, Item item)
        {
            this.items.Add(key, item);
        }

        private void Show()
        {
            this.Output("You are carrying:");
            int count = this.items.Look("{0}");
            if (count == 0)
            {
                this.Output("(nothing)");
            }
        }

        private void Add(Word verb, Word noun, Item item)
        {
            this.Add(noun.Primary, item);
            this.Output($"You {verb} the {noun}.");
        }

        private void Drop(Word verb, Word noun, Items targetItems)
        {
            Item item = this.items.Take(noun.Primary);
            if (item == null)
            {
                this.Output($"You can't {verb} what you don't have.");
                return;
            }

            if (item.Drop(this.bus))
            {
                targetItems.Add(noun.Primary, item);
                this.Output($"You {verb} the {noun}.");
                return;
            }

            this.items.Add(noun.Primary, item);
        }

        private void Output(string text)
        {
            this.bus.Send(new OutputMessage(text));
        }
    }
}
