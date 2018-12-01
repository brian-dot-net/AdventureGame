// <copyright file="Word.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    public sealed class Word
    {
        public Word(string primary, string actual)
        {
            this.Primary = primary;
            this.Actual = actual;
        }

        public string Primary { get; }

        public string Actual { get; }

        public override string ToString() => this.Actual;
    }
}
