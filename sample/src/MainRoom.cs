// <copyright file="MainRoom.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Sample
{
    internal sealed class MainRoom : RoomBase
    {
        public MainRoom(MessageBus bus)
            : base(bus)
        {
            this.Add(Noun.Table, new Table(bus, this));
        }

        protected override string Description => "You are in the main room. There is a doorway to the east.";
    }
}
