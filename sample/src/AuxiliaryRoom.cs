// <copyright file="AuxiliaryRoom.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Sample
{
    internal sealed class AuxiliaryRoom : RoomBase
    {
        public AuxiliaryRoom(MessageBus bus)
            : base(bus)
        {
        }

        protected override string Description => "You are in the auxiliary room. There is a hole in the wall. There is a doorway to the west. There is a cliff to the east.";

        protected override bool GoCore(Word noun)
        {
            if (noun.Primary == Noun.East)
            {
                this.End("You fall off a cliff. Game over.");
                return true;
            }

            return base.GoCore(noun);
        }

        protected override bool LookAtCore(Word noun)
        {
            if (noun.Primary == Noun.Wall)
            {
                this.Output("Above the hole you see the words, \"INSERT COIN.\"");
                return true;
            }

            return base.LookAtCore(noun);
        }
    }
}
