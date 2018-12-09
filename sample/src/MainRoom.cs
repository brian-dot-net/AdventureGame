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
        }

        protected override string Description => "You are in the main room. There is a table here.";

        protected override void EnterCore()
        {
            this.Register(Verb.Look, (_, n) => this.Look(n));
            this.Register(Verb.Greet, (_, __) => this.Output("You say, \"Hello,\" to no one in particular. No one answers."));
            this.Register(Verb.Move, (_, n) => this.Move(n));
            this.Register(Verb.Take, this.Take);
        }

        protected override bool LookAt(Word noun)
        {
            if (noun.Primary == Noun.Table)
            {
                this.Output("It is an ordinary wooden table.");
                return true;
            }

            return base.LookAt(noun);
        }

        private void Move(Word noun)
        {
            if (noun.Primary == Noun.Table)
            {
                this.Output("You move the table slightly. Underneath you see a coin.");
            }
        }
    }
}
