// <copyright file="Item.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    using Adventure.Messages;

    public abstract class Item
    {
        private readonly MessageBus bus;

        protected Item(MessageBus bus)
        {
            this.bus = bus;
        }

        public abstract string ShortDescription { get; }

        public abstract string LongDescription { get; }

        public bool Do(MessageBus bus, Word verb, Word noun)
        {
            return this.DoCore(bus, verb, noun);
        }

        public bool Take()
        {
            return this.TakeCore();
        }

        public bool Drop(MessageBus bus)
        {
            return this.DropCore(bus);
        }

        protected virtual bool DoCore(MessageBus bus, Word verb, Word noun)
        {
            return false;
        }

        protected virtual bool TakeCore()
        {
            return true;
        }

        protected virtual bool DropCore(MessageBus bus)
        {
            return true;
        }

        protected void Output(string text)
        {
            this.bus.Send(new OutputMessage(text));
        }
    }
}
