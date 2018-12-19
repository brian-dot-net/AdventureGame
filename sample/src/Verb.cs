// <copyright file="Verb.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Sample
{
    using System.Reflection;

    internal sealed class Verb
    {
        public static readonly Verb Drop = new Verb("drop", "throw");
        public static readonly Verb Go = new Verb("go");
        public static readonly Verb Greet = new Verb("greet", "hello", "hi");
        public static readonly Verb Inventory = new Verb("inventory", "inv");
        public static readonly Verb Look = new Verb("look");
        public static readonly Verb Move = new Verb("move");
        public static readonly Verb Take = new Verb("take", "get");
        public static readonly Verb Quit = new Verb("quit", "exit");
        public static readonly Verb Read = new Verb("read");

        private readonly string verb;
        private readonly string[] synonyms;

        private Verb(string verb, params string[] synonyms)
        {
            this.verb = verb;
            this.synonyms = synonyms;
        }

        public static implicit operator string(Verb v) => v.verb;

        public static void Register(Words words)
        {
            foreach (FieldInfo fi in typeof(Verb).GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                Verb v = (Verb)fi.GetValue(null);
                v.AddTo(words);
            }
        }

        private void AddTo(Words words)
        {
            words.Add(this.verb, this.synonyms);
        }
    }
}
