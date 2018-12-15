// <copyright file="Item.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    using Adventure.Messages;

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

        public bool Take(MessageBus bus)
        {
            return this.TakeCore(bus);
        }

        public bool Drop(MessageBus bus)
        {
            return this.DropCore(bus);
        }

        protected virtual bool DoCore(MessageBus bus, Word verb, Word noun)
        {
            return false;
        }

        protected virtual bool TakeCore(MessageBus bus)
        {
            return true;
        }

        protected virtual bool DropCore(MessageBus bus)
        {
            return true;
        }

        protected void Output(MessageBus bus, string text)
        {
            bus.Send(new OutputMessage(text));
        }
    }
}
