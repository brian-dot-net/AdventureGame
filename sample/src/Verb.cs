// <copyright file="Verb.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Sample
{
    internal sealed class Verb
    {
        public static readonly Verb Drop = new Verb("drop");
        public static readonly Verb Go = new Verb("go");
        public static readonly Verb Greet = new Verb("greet");
        public static readonly Verb Inventory = new Verb("inventory");
        public static readonly Verb Look = new Verb("look");
        public static readonly Verb Move = new Verb("move");
        public static readonly Verb Take = new Verb("take");
        public static readonly Verb Quit = new Verb("quit");
        public static readonly Verb Read = new Verb("read");

        private readonly string verb;

        private Verb(string verb)
        {
            this.verb = verb;
        }

        public static implicit operator string(Verb v) => v.verb;
    }
}
