// <copyright file="Items.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    using System;
    using System.Collections.Generic;

    public sealed class Items
    {
        private readonly Dictionary<string, Item> items;

        public Items()
        {
            this.items = new Dictionary<string, Item>();
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
    }
}
