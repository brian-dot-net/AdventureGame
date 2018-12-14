// <copyright file="InventoryDropMessage.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    public sealed class InventoryDropMessage
    {
        public InventoryDropMessage(Items items, Word verb, Word noun)
        {
            this.Items = items;
            this.Verb = verb;
            this.Noun = noun;
        }

        public Items Items { get; }

        public Word Verb { get; }

        public Word Noun { get; }
    }
}
