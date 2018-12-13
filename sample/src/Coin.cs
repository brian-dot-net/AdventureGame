// <copyright file="Coin.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Sample
{
    internal sealed class Coin : Item
    {
        public override string ShortDescription => "a coin";

        public override string LongDescription => "It is a small gold coin with an inscription on the edge.";
    }
}
