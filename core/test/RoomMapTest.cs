// <copyright file="RoomMapTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Test
{
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
    }
}
