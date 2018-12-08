// <copyright file="RoomMap.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    using System;
    using System.Collections.Generic;

    public sealed class RoomMap
    {
        private Point current;

        public Point Add(Room room)
        {
            return new Point(room);
        }

        public void Start(Point start)
        {
            if (this.current != null)
            {
                throw new InvalidOperationException("Cannot Start again.");
            }

            this.Next(start);
        }

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
            private readonly Dictionary<string, Point> targets;

            public Point(Room room)
            {
                this.room = room;
                this.targets = new Dictionary<string, Point>();
            }

            public void ConnectTo(Point target, string direction)
            {
                this.targets.Add(direction, target);
            }

            public void Enter() => this.room.Enter();

            public void Leave() => this.room.Leave();

            public Point Go(string direction) => this.targets[direction];
        }
    }
}
