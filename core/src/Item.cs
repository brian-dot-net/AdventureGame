// <copyright file="Item.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    public abstract class Item
    {
        protected Item()
        {
        }

        public abstract string ShortDescription { get; }

        public abstract string LongDescription { get; }

        public bool Do(MessageBus bus, Word verb, Word noun)
        {
            return this.DoCore(bus, verb, noun);
        }

        protected virtual bool DoCore(MessageBus bus, Word verb, Word noun)
        {
            return false;
        }
    }
}
