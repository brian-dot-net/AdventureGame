// <copyright file="Items.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    using System.Collections.Generic;

    public sealed class Items
    {
        private readonly Dictionary<string, Item> items;

        public Items()
        {
            this.items = new Dictionary<string, Item>();
        }

        public void Add(string name, Item item)
        {
            this.items.Add(name, item);
        }

        public Item Take(string name)
        {
            this.items.Remove(name, out Item item);
            return item;
        }
    }
}
