// <copyright file="OutputMessage.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    public sealed class OutputMessage
    {
        public OutputMessage(string text)
        {
            this.Text = text;
        }

        public string Text { get; }
    }
}
