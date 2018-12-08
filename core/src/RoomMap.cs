// <copyright file="RoomMap.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    using System;
    using System.Collections.Generic;

    public sealed class RoomMap
    {
        private readonly MessageBus bus;

        private IPointPrivate current;

        public RoomMap(MessageBus bus)
        {
            this.bus = bus;
        }

        private interface IPointPrivate
        {
            void Enter();

            void Leave();

            Point Go(string direction);
        }

        public Point Add(Room room)
        {
            return new Point(this.bus, room);
        }

        public void Start(Point start)
        {
            if (this.current != null)
            {
                throw new InvalidOperationException("Cannot Start again.");
            }

            this.Next(start);
        }

        public void Go(string direction)
        {
            if (this.current == null)
            {
                throw new InvalidOperationException("Cannot Go before Start.");
            }

            this.Next(this.current.Go(direction));
        }

        private void Next(Point next)
        {
            if (this.current != next)
            {
                this.current?.Leave();
                this.current = next;
                this.current.Enter();
            }
        }

        public sealed class Point : IPointPrivate
        {
            private readonly MessageBus bus;
            private readonly Room room;
            private readonly Dictionary<string, Point> targets;

            public Point(MessageBus bus, Room room)
            {
                this.bus = bus;
                this.room = room;
                this.targets = new Dictionary<string, Point>();
            }

            public void ConnectTo(Point target, string direction)
            {
                if (this.targets.ContainsKey(direction))
                {
                    throw new InvalidOperationException($"There is already a connection for '{direction}'.");
                }

                this.targets.Add(direction, target);
            }

            void IPointPrivate.Enter() => this.room.Enter();

            void IPointPrivate.Leave() => this.room.Leave();

            Point IPointPrivate.Go(string direction)
            {
                if (!this.targets.TryGetValue(direction, out Point target))
                {
                    this.bus.Send(new OutputMessage($"You can't go {direction}."));
                    target = this;
                }

                return target;
            }
        }
    }
}
