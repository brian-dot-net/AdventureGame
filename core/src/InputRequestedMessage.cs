// <copyright file="InputRequestedMessage.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    public sealed class InputRequestedMessage
    {
        public InputRequestedMessage(string prompt = null)
        {
            this.Prompt = prompt;
        }

        public string Prompt { get; }
    }
}
