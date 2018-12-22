// <copyright file="EndOfGameMessage.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Messages
{
    public sealed class EndOfGameMessage
    {
        public EndOfGameMessage(string text = null)
        {
            this.Text = text;
        }

        public string Text { get; }
    }
}
