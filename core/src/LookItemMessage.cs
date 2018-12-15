// <copyright file="LookItemMessage.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    public sealed class LookItemMessage
    {
        public LookItemMessage(Word noun)
        {
            this.Noun = noun;
        }

        public Word Noun { get; }
    }
}
