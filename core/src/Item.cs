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

        public bool Do(Word verb, Word noun)
        {
            return this.DoCore(verb, noun);
        }

        public bool Take()
        {
            return this.TakeCore();
        }

        public bool Drop()
        {
            return this.DropCore();
        }

        protected virtual bool DoCore(Word verb, Word noun)
        {
            return false;
        }

        protected virtual bool TakeCore()
        {
            return true;
        }

        protected virtual bool DropCore()
        {
            return true;
        }

        protected void Output(string text)
        {
            this.Bus.Send(new OutputMessage(text));
        }
    }
}
