// <copyright file="Noun.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Sample
{
    internal sealed class Noun
    {
        public static readonly Noun Coin = new Noun("coin");
        public static readonly Noun East = new Noun("east");
        public static readonly Noun Table = new Noun("table");
        public static readonly Noun West = new Noun("west");

        private readonly string noun;

        private Noun(string noun)
        {
            this.noun = noun;
        }

        public static implicit operator string(Noun n) => n.noun;
    }
}
