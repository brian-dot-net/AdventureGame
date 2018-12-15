// <copyright file="Coin.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Sample
{
    internal sealed class Coin : Item
    {
        private bool taken;

        public Coin(MessageBus bus)
            : base(bus)
        {
        }

        public override string ShortDescription => "a coin";

        public override string LongDescription => "It is a small gold coin with an inscription on the edge.";

        protected override bool TakeCore()
        {
            this.taken = true;
            return base.TakeCore();
        }

        protected override bool DropCore(MessageBus bus)
        {
            this.taken = false;
            return base.DropCore(bus);
        }

        protected override bool DoCore(MessageBus bus, Word verb, Word noun)
        {
            if (verb.Primary == Verb.Read)
            {
                this.Read();
                return true;
            }

            return base.DoCore(bus, verb, noun);
        }

        private void Read()
        {
            if (this.taken)
            {
                this.Output("The inscription reads: \"MCMXCIX\"");
            }
            else
            {
                this.Output("The writing is too small. You'd have to pick it up to see it better.");
            }
        }
    }
}
