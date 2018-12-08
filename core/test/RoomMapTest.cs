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
            RoomMap map = new RoomMap();

            RoomMap.Point p1 = map.Add(new TestRoom(bus));
            RoomMap.Point p2 = map.Add(new TestRoom(bus));

            p1.Should().NotBeSameAs(p2);
        }

        [Fact]
        public void ConnectTwoRoomsOneDirection()
        {
            MessageBus bus = new MessageBus();
            RoomMap map = new RoomMap();
            RoomMap.Point p1 = map.Add(new TestRoom(bus));
            RoomMap.Point p2 = map.Add(new TestRoom(bus));

            Action act = () => p1.ConnectTo(p2, "east");

            act.Should().NotThrow();
        }

        [Fact]
        public void GoBetweenTwoRooms()
        {
            MessageBus bus = new MessageBus();
            List<string> messages = new List<string>();
            bus.Subscribe<OutputMessage>(m => messages.Add(m.Text));
            RoomMap map = new RoomMap();
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
    }
}
