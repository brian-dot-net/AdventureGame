// <copyright file="TestRoom.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Test
{
    public sealed class TestRoom : Room
    {
        public TestRoom(MessageBus bus)
            : base(bus)
        {
        }

        protected override string Description => "You are in a test room.";

        public void TestRegisterHello(string verb)
        {
            this.Register(verb, this.Hello);
        }

        protected override void EnterCore()
        {
            this.TestRegisterHello("hello");
            this.Register("look", (_, n) => this.Look(n));
        }

        protected override bool LookAt(Word noun)
        {
            if (noun.Primary == "up")
            {
                this.Output("You see the ceiling.");
                return true;
            }

            return base.LookAt(noun);
        }

        private void Hello(Word verb, Word noun)
        {
            this.Output("Hello, " + noun + "!");
        }
    }
}
