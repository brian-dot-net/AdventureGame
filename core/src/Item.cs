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

        public void Do(MessageBus bus, Word verb, Word noun)
        {
            this.DoCore(bus, verb, noun);
        }

        protected virtual void DoCore(MessageBus bus, Word verb, Word noun)
        {
        }
    }
}
