// <copyright file="RoomMapTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Test
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using Xunit;

    public sealed class RoomMapTest
    {
        [Fact]
        public void AddTwoRooms()
        {
            MessageBus bus = new MessageBus();
            RoomMap map = new RoomMap(bus);

            RoomMap.Point p1 = map.Add(new TestRoom(bus));
            RoomMap.Point p2 = map.Add(new TestRoom(bus));

            p1.Should().NotBeSameAs(p2);
        }

        [Fact]
        public void StartTwice()
        {
            MessageBus bus = new MessageBus();
            RoomMap map = new RoomMap(bus);
            RoomMap.Point p1 = map.Add(new TestRoom(bus));
            RoomMap.Point p2 = map.Add(new TestRoom(bus));

            map.Start(p1);
            Action act = () => map.Start(p2);

            act.Should().Throw<InvalidOperationException>().WithMessage("Cannot Start again.");
        }

        [Fact]
        public void ConnectTwoRoomsOneDirection()
        {
            MessageBus bus = new MessageBus();
            RoomMap map = new RoomMap(bus);
            RoomMap.Point p1 = map.Add(new TestRoom(bus));
            RoomMap.Point p2 = map.Add(new TestRoom(bus));

            Action act = () => p1.ConnectTo(p2, "east");

            act.Should().NotThrow();
        }

        [Fact]
        public void ConnectTwice()
        {
            MessageBus bus = new MessageBus();
            RoomMap map = new RoomMap(bus);
            RoomMap.Point p1 = map.Add(new TestRoom(bus));
            RoomMap.Point p2 = map.Add(new TestRoom(bus));

            p1.ConnectTo(p2, "east");
            Action act = () => p1.ConnectTo(p2, "east");

            act.Should().Throw<InvalidOperationException>("There is already a connection for 'east'.");
        }

        [Fact]
        public void GoBetweenTwoRooms()
        {
            MessageBus bus = new MessageBus();
            List<string> messages = new List<string>();
            bus.Subscribe<OutputMessage>(m => messages.Add(m.Text));
            RoomMap map = new RoomMap(bus);
            RoomMap.Point p1 = map.Add(new TestRoom(bus, "red"));
            RoomMap.Point p2 = map.Add(new TestRoom(bus, "green"));
            p1.ConnectTo(p2, "east");
            p2.ConnectTo(p1, "west");

            map.Start(p1);
            map.Go("east");
            map.Go("west");

            messages.Should().Equal(
                "You are in a red test room.",
                "You are in a green test room.",
                "You are in a red test room.");
        }

        [Fact]
        public void GoWrongWayAtFirst()
        {
            MessageBus bus = new MessageBus();
            List<string> messages = new List<string>();
            bus.Subscribe<OutputMessage>(m => messages.Add(m.Text));
            RoomMap map = new RoomMap(bus);
            RoomMap.Point p1 = map.Add(new TestRoom(bus, "red"));
            RoomMap.Point p2 = map.Add(new TestRoom(bus, "green"));
            p1.ConnectTo(p2, "east");
            p2.ConnectTo(p1, "west");

            map.Start(p1);
            map.Go("north");
            map.Go("east");

            messages.Should().Equal(
                "You are in a red test room.",
                "You can't go north.",
                "You are in a green test room.");
        }

        [Fact]
        public void GoBeforeStart()
        {
            MessageBus bus = new MessageBus();
            List<string> messages = new List<string>();
            bus.Subscribe<OutputMessage>(m => messages.Add(m.Text));
            RoomMap map = new RoomMap(bus);
            RoomMap.Point p1 = map.Add(new TestRoom(bus, "red"));
            RoomMap.Point p2 = map.Add(new TestRoom(bus, "green"));
            p1.ConnectTo(p2, "east");
            p2.ConnectTo(p1, "west");

            Action act = () => map.Go("east");

            act.Should().Throw<InvalidOperationException>("Cannot Go before Start.");
        }

        [Fact]
        public void GoBetweenFourRoomsBothDirections()
        {
            MessageBus bus = new MessageBus();
            List<string> messages = new List<string>();
            bus.Subscribe<OutputMessage>(m => messages.Add(m.Text));
            RoomMap map = new RoomMap(bus);
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
            map.Go("east");
            map.Go("south");
            map.Go("west");
            map.Go("north");
            map.Go("south");
            map.Go("east");
            map.Go("north");
            map.Go("west");

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
}
