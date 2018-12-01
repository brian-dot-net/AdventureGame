// <copyright file="SentenceMessage.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    public sealed class SentenceMessage
    {
        public SentenceMessage(Word verb, Word noun)
        {
            this.Verb = verb;
            this.Noun = noun;
        }

        public Word Verb { get; }

        public Word Noun { get; }
    }
}
