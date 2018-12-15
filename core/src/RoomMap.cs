// <copyright file="RoomMap.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    using System;
    using System.Collections.Generic;
    using Adventure.Messages;

    public sealed class RoomMap : IDisposable
    {
        private readonly MessageBus bus;

        private IDisposable sub;
        private IPointPrivate current;

        public RoomMap(MessageBus bus)
        {
            this.bus = bus;
        }

        private interface IPointPrivate
        {
            RoomMap Parent { get; set; }

            void Enter();

            void Leave();

            Point Go(string direction);
        }

        public void Dispose()
        {
            using (this.sub)
            {
            }
        }

        public Point Add(Room room)
        {
            Point point = new Point(this.bus, room);
            ((IPointPrivate)point).Parent = this;
            return point;
        }

        public void Start(Point start)
        {
            if (((IPointPrivate)start).Parent != this)
            {
                throw new ArgumentException("The point is not part of this map.", nameof(start));
            }

            if (this.sub != null)
            {
                throw new InvalidOperationException("Cannot Start again.");
            }

            this.sub = this.bus.Subscribe<GoMessage>(m => this.Go(m.Direction));

            this.Next(start);
        }

        private void Go(string direction)
        {
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

            RoomMap IPointPrivate.Parent { get; set; }

            public void ConnectTo(Point target, string direction)
            {
                if (((IPointPrivate)this).Parent != ((IPointPrivate)target).Parent)
                {
                    throw new ArgumentException("The point is not part of this map.", nameof(target));
                }

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
