﻿// <copyright file="Table.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Sample
{
    internal sealed class Table : Item
    {
        private readonly Room parent;

        private bool tableMoved;

        public Table(Room parent)
        {
            this.parent = parent;
        }

        public override string ShortDescription => "a table";

        public override string LongDescription => "It is an ordinary wooden table.";

        protected override bool TakeCore(MessageBus bus)
        {
            bus.Send(new OutputMessage("It is too heavy."));
            return false;
        }

        protected override bool DoCore(MessageBus bus, Word verb, Word noun)
        {
            if (verb.Primary == Verb.Move)
            {
                this.Move(bus);
                return true;
            }

            return base.DoCore(bus, verb, noun);
        }

        private void Move(MessageBus bus)
        {
            if (!this.tableMoved)
            {
                this.tableMoved = true;
                bus.Send(new OutputMessage("You move the table slightly. Underneath you see a coin."));
                this.parent.Add(Noun.Coin, new Coin());
            }
            else
            {
                bus.Send(new OutputMessage("Someone has already moved it."));
            }
        }
    }
}
