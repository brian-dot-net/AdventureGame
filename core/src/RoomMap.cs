// <copyright file="RoomMap.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    using System;

    public sealed class RoomMap
    {
        private Point current;

        public Point Add(Room room)
        {
            return new Point(room);
        }

        public void Start(Point start) => this.Next(start);

        public void Go(string direction) => this.Next(this.current.Go(direction));

        private void Next(Point next)
        {
            this.current?.Leave();
            this.current = next;
            this.current.Enter();
        }

        public sealed class Point
        {
            private readonly Room room;

            private Point target;

            public Point(Room room)
            {
                this.room = room;
            }

            public void ConnectTo(Point target, string direction)
            {
                this.target = target;
            }

            public void Enter() => this.room.Enter();

            public void Leave() => this.room.Leave();

            public Point Go(string direction) => this.target;
        }
    }
}
