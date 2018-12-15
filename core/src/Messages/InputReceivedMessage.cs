// <copyright file="InputReceivedMessage.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Messages
{
    public sealed class InputReceivedMessage
    {
        public InputReceivedMessage(string line)
        {
            this.Line = line;
        }

        public string Line { get; }
    }
}
