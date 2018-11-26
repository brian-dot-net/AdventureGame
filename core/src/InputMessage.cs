// <copyright file="InputMessage.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    public sealed class InputMessage
    {
        public InputMessage(string line)
        {
            this.Line = line;
        }

        public string Line { get; }
    }
}
