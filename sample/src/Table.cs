// <copyright file="Table.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Sample
{
    internal sealed class Table : Item
    {
        private readonly Room parent;

        private bool tableMoved;

        public Table(MessageBus bus, Room parent)
            : base(bus)
        {
            this.parent = parent;
        }

        public override string ShortDescription => "a table";

        public override string LongDescription => "It is an ordinary wooden table.";

        protected override bool TakeCore()
        {
            this.Output("It is too heavy.");
            return false;
        }

        protected override bool DoCore(Word verb, Word noun)
        {
            if (verb.Primary == Verb.Move)
            {
                this.Move();
                return true;
            }

            return base.DoCore(verb, noun);
        }

        private void Move()
        {
            if (!this.tableMoved)
            {
                this.tableMoved = true;
                this.Output("You move the table slightly. Underneath you see a coin.");
                this.parent.Add(Noun.Coin, new Coin(this.Bus));
            }
            else
            {
                this.Output("Someone has already moved it.");
            }
        }
    }
}
