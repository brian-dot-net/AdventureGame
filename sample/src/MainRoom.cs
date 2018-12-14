// <copyright file="MainRoom.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Sample
{
    internal sealed class MainRoom : Room
    {
        public MainRoom(MessageBus bus)
            : base(bus)
        {
            this.Add(Noun.Table, new Table(this));
        }

        protected override string Description => "You are in the main room.";

        protected override void EnterCore()
        {
            this.Register(Verb.Look, (_, n) => this.Look(n));
            this.Register(Verb.Greet, (_, __) => this.Output("You say, \"Hello,\" to no one in particular. No one answers."));
            this.Register(Verb.Inventory, (_, __) => this.Inventory());
            this.Register(Verb.Take, this.Take);
        }
    }
}
