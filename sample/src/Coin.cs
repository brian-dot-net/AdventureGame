// <copyright file="Coin.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Sample
{
    internal sealed class Coin : Item
    {
        private bool taken;

        public override string ShortDescription => "a coin";

        public override string LongDescription => "It is a small gold coin with an inscription on the edge.";

        protected override bool TakeCore(MessageBus bus)
        {
            this.taken = true;
            return base.TakeCore(bus);
        }

        protected override bool DoCore(MessageBus bus, Word verb, Word noun)
        {
            if (verb.Primary == Verb.Read)
            {
                this.Read(bus);
                return true;
            }

            return base.DoCore(bus, verb, noun);
        }

        private void Read(MessageBus bus)
        {
            if (this.taken)
            {
                bus.Send(new OutputMessage("The inscription reads: \"MCMXCIX\""));
            }
            else
            {
                bus.Send(new OutputMessage("The writing is too small. You'd have to pick it up to see it better."));
            }
        }
    }
}
