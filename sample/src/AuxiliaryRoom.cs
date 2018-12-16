// <copyright file="AuxiliaryRoom.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Sample
{
    internal sealed class AuxiliaryRoom : Room
    {
        public AuxiliaryRoom(MessageBus bus)
            : base(bus)
        {
        }

        protected override string Description => "You are in the auxiliary room. There is a doorway to the west.";

        protected override void EnterCore()
        {
            this.Register(Verb.Drop, this.Drop);
            this.Register(Verb.Go, this.Go);
            this.Register(Verb.Greet, (_, __) => this.Output("You say, \"Hello,\" to no one in particular. No one answers."));
            this.Register(Verb.Look, (_, n) => this.Look(n));
            this.Register(Verb.Inventory, (_, __) => this.Inventory());
            this.Register(Verb.Take, this.Take);
        }
    }
}
