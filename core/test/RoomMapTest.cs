// <copyright file="RoomMapTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Test
{
    using System;
    using System.Collections.Generic;
    using Adventure.Messages;
    using FluentAssertions;
    using Xunit;

    public sealed class RoomMapTest
    {
        [Fact]
        public void DisposeEmpty()
        {
            MessageBus bus = new MessageBus();
            RoomMap map = new RoomMap(bus);

            Action act = () => map.Dispose();

            act.Should().NotThrow();
        }

        [Fact]
        public void AddTwoRooms()
        {
            MessageBus bus = new MessageBus();
            using (RoomMap map = new RoomMap(bus))
            {
                RoomMap.Point p1 = map.Add(new TestRoom(bus));
                RoomMap.Point p2 = map.Add(new TestRoom(bus));

                p1.Should().NotBeSameAs(p2);
            }
        }

        [Fact]
        public void StartTwice()
        {
            MessageBus bus = new MessageBus();
            using (RoomMap map = new RoomMap(bus))
            {
                RoomMap.Point p1 = map.Add(new TestRoom(bus));
                RoomMap.Point p2 = map.Add(new TestRoom(bus));

                map.Start(p1);
                Action act = () => map.Start(p2);

                act.Should().Throw<InvalidOperationException>().WithMessage("Cannot Start again.");
            }
        }

        [Fact]
        public void ConnectTwoRoomsOneDirection()
        {
            MessageBus bus = new MessageBus();
            using (RoomMap map = new RoomMap(bus))
            {
                RoomMap.Point p1 = map.Add(new TestRoom(bus));
                RoomMap.Point p2 = map.Add(new TestRoom(bus));

                Action act = () => p1.ConnectTo(p2, "east");

                act.Should().NotThrow();
            }
        }

        [Fact]
        public void ConnectTwice()
        {
            MessageBus bus = new MessageBus();
            using (RoomMap map = new RoomMap(bus))
            {
                RoomMap.Point p1 = map.Add(new TestRoom(bus));
                RoomMap.Point p2 = map.Add(new TestRoom(bus));

                p1.ConnectTo(p2, "east");
                Action act = () => p1.ConnectTo(p2, "east");

                act.Should().Throw<InvalidOperationException>("There is already a connection for 'east'.");
            }
        }

        [Fact]
        public void GoBetweenTwoRooms()
        {
            MessageBus bus = new MessageBus();
            List<string> messages = new List<string>();
            bus.Subscribe<OutputMessage>(m => messages.Add(m.Text));
            using (RoomMap map = new RoomMap(bus))
            {
                RoomMap.Point p1 = map.Add(new TestRoom(bus, "red"));
                RoomMap.Point p2 = map.Add(new TestRoom(bus, "green"));
                p1.ConnectTo(p2, "east");
                p2.ConnectTo(p1, "west");

                map.Start(p1);
                bus.Send(new GoMessage("east"));
                bus.Send(new GoMessage("west"));

                messages.Should().Equal(
                    "You are in a red test room.",
                    "You are in a green test room.",
                    "You are in a red test room.");
            }
        }

        [Fact]
        public void GoWrongWayAtFirst()
        {
            MessageBus bus = new MessageBus();
            List<string> messages = new List<string>();
            bus.Subscribe<OutputMessage>(m => messages.Add(m.Text));
            using (RoomMap map = new RoomMap(bus))
            {
                RoomMap.Point p1 = map.Add(new TestRoom(bus, "red"));
                RoomMap.Point p2 = map.Add(new TestRoom(bus, "green"));
                p1.ConnectTo(p2, "east");
                p2.ConnectTo(p1, "west");

                map.Start(p1);
                bus.Send(new GoMessage("north"));
                bus.Send(new GoMessage("east"));

                messages.Should().Equal(
                    "You are in a red test room.",
                    "You can't go north.",
                    "You are in a green test room.");
            }
        }

        [Fact]
        public void GoAfterDispose()
        {
            MessageBus bus = new MessageBus();
            List<string> messages = new List<string>();
            bus.Subscribe<OutputMessage>(m => messages.Add(m.Text));
            using (RoomMap map = new RoomMap(bus))
            {
                RoomMap.Point p1 = map.Add(new TestRoom(bus, "red"));
                map.Start(p1);
            }

            bus.Send(new GoMessage("nowhere"));

            messages.Should().Equal("You are in a red test room.");
        }

        [Fact]
        public void GoBetweenFourRoomsBothDirections()
        {
            MessageBus bus = new MessageBus();
            List<string> messages = new List<string>();
            bus.Subscribe<OutputMessage>(m => messages.Add(m.Text));
            using (RoomMap map = new RoomMap(bus))
            {
                RoomMap.Point nw = map.Add(new TestRoom(bus, "NW"));
                RoomMap.Point ne = map.Add(new TestRoom(bus, "NE"));
                RoomMap.Point sw = map.Add(new TestRoom(bus, "SW"));
                RoomMap.Point se = map.Add(new TestRoom(bus, "SE"));
                nw.ConnectTo(ne, "east");
                nw.ConnectTo(sw, "south");
                ne.ConnectTo(nw, "west");
                ne.ConnectTo(se, "south");
                sw.ConnectTo(se, "east");
                sw.ConnectTo(nw, "north");
                se.ConnectTo(sw, "west");
                se.ConnectTo(ne, "north");

                map.Start(nw);
                bus.Send(new GoMessage("east"));
                bus.Send(new GoMessage("south"));
                bus.Send(new GoMessage("west"));
                bus.Send(new GoMessage("north"));
                bus.Send(new GoMessage("south"));
                bus.Send(new GoMessage("east"));
                bus.Send(new GoMessage("north"));
                bus.Send(new GoMessage("west"));

                messages.Should().Equal(
                    "You are in a NW test room.",
                    "You are in a NE test room.",
                    "You are in a SE test room.",
                    "You are in a SW test room.",
                    "You are in a NW test room.",
                    "You are in a SW test room.",
                    "You are in a SE test room.",
                    "You are in a NE test room.",
                    "You are in a NW test room.");
            }
        }

        [Fact]
        public void StartBadPoint()
        {
            MessageBus bus = new MessageBus();
            using (RoomMap map = new RoomMap(bus))
            using (RoomMap mapWrong = new RoomMap(bus))
            {
                var wrongP = mapWrong.Add(new TestRoom(bus));
                Action act = () => map.Start(wrongP);

                act.Should().Throw<ArgumentException>()
                    .WithMessage("The point is not part of this map.*")
                    .Which.ParamName.Should().Be("start");
            }
        }

        [Fact]
        public void ConnectToBadPoint()
        {
            MessageBus bus = new MessageBus();
            using (RoomMap map = new RoomMap(bus))
            using (RoomMap mapWrong = new RoomMap(bus))
            {
                var rightP = map.Add(new TestRoom(bus));
                var wrongP = mapWrong.Add(new TestRoom(bus));
                Action act = () => rightP.ConnectTo(wrongP, "north");

                act.Should().Throw<ArgumentException>()
                    .WithMessage("The point is not part of this map.*")
                    .Which.ParamName.Should().Be("target");
            }
        }

        [Fact]
        public void ConnectRoomToItself()
        {
            MessageBus bus = new MessageBus();
            List<string> messages = new List<string>();
            bus.Subscribe<OutputMessage>(m => messages.Add(m.Text));
            using (RoomMap map = new RoomMap(bus))
            {
                var p1 = map.Add(new TestRoom(bus));

                p1.ConnectTo(p1, "north");
                map.Start(p1);
                bus.Send(new GoMessage(string.Empty));
                bus.Send(new GoMessage("north"));

                messages.Should().Equal(
                    "You are in a test room.",
                    "You can't go that way.",
                    "You are in a test room.");
            }
        }
    }
}
