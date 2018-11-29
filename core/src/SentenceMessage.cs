// <copyright file="SentenceMessage.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    public sealed class SentenceMessage
    {
        private readonly string line;

        public SentenceMessage(string line)
        {
            this.line = line;
        }

        public override string ToString() => this.line;
    }
}
