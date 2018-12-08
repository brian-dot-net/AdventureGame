// <copyright file="RoomMap.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    using System;

    public sealed class RoomMap
    {
        public Point Add(Room room)
        {
            return new Point(room);
        }

        public sealed class Point
        {
            private readonly Room room;

            public Point(Room room)
            {
                this.room = room;
            }

            public void ConnectTo(Point target, string direction)
            {
            }
        }
    }
}
