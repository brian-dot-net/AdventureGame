// <copyright file="GoMessage.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    public sealed class GoMessage
    {
        public GoMessage(string direction)
        {
            this.Direction = direction;
        }

        public string Direction { get; }
    }
}
