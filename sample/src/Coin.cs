// <copyright file="Coin.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Sample
{
    internal sealed class Coin : Item
    {
        public Coin(MessageBus bus)
            : base(bus)
        {
        }

        public override string ShortDescription => "a coin";

        public override string LongDescription => "It is a small gold coin with an inscription on the edge.";

        protected override bool DoCore(Word verb, Word noun)
        {
            if (verb.Primary == Verb.Read)
            {
                this.Read();
                return true;
            }

            return base.DoCore(verb, noun);
        }

        private void Read()
        {
            if (this.Taken)
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
