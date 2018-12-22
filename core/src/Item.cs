// <copyright file="Item.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    using Adventure.Messages;

    public abstract class Item
    {
        protected Item(MessageBus bus)
        {
            this.Bus = bus;
        }

        public abstract string ShortDescription { get; }

        public abstract string LongDescription { get; }

        protected MessageBus Bus { get; }

        protected bool Taken { get; private set; }

        public bool Do(Word verb, Word noun) => this.DoCore(verb, noun);

        public bool Take()
        {
            return this.Taken = this.TakeCore();
        }

        public bool Drop()
        {
            this.Taken = false;
            return this.DropCore();
        }

        protected virtual bool DoCore(Word verb, Word noun) => false;

        protected virtual bool TakeCore() => true;

        protected virtual bool DropCore() => true;

        protected void Output(string text) => this.Bus.Output(text);
    }
}
