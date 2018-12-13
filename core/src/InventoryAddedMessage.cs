// <copyright file="InventoryAddedMessage.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    public sealed class InventoryAddedMessage
    {
        public InventoryAddedMessage(Word verb, Word noun, Item item)
        {
            this.Verb = verb;
            this.Noun = noun;
            this.Item = item;
        }

        public Word Verb { get; }

        public Word Noun { get; }

        public Item Item { get; }
    }
}
