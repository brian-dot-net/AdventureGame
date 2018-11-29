// <copyright file="SentenceMessage.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    public sealed class SentenceMessage
    {
        public SentenceMessage(string verb, string noun)
        {
            this.Verb = verb;
            this.Noun = noun;
        }

        public string Verb { get; }

        public string Noun { get; }
    }
}
