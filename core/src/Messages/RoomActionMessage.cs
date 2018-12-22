// <copyright file="RoomActionMessage.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Messages
{
    using System;

    public sealed class RoomActionMessage
    {
        public RoomActionMessage(Action<Room> act)
        {
            this.Act = act;
        }

        public Action<Room> Act { get; }
    }
}
