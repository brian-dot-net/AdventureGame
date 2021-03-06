﻿// <copyright file="TestRoom.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Test
{
    using System.Collections.Generic;

    public sealed class TestRoom : Room
    {
        private readonly string color;

        public TestRoom(MessageBus bus, string color = null)
            : base(bus)
        {
            this.color = color;
        }

        protected override string Description
        {
            get
            {
                List<string> parts = new List<string>();
                parts.Add("You are in a");
                if (this.color != null)
                {
                    parts.Add(this.color);
                }

                parts.Add("test room.");

                return string.Join(" ", parts);
            }
        }

        public void TestRegisterHello(string verb)
        {
            this.Register(verb, this.Hello);
        }

        protected override void EnterCore()
        {
            this.TestRegisterHello("hello");
            this.Register("look", (_, n) => this.Look(n));
            this.Register("take", this.Take);
            this.Register("inventory", (_, __) => this.Inventory());
            this.Register("drop", this.Drop);
            this.Register("go", this.Go);
            this.Register("quit", (_, __) => this.End("You quit. Game over."));
        }

        protected override bool LookAtCore(Word noun)
        {
            if (noun.Primary == "up")
            {
                this.Output("You see the ceiling.");
                return true;
            }

            return base.LookAtCore(noun);
        }

        protected override bool TakeCore(Word noun)
        {
            if (noun.Primary == "breath")
            {
                this.Output("You inhale deeply.");
                return true;
            }

            return base.TakeCore(noun);
        }

        protected override bool GoCore(Word noun)
        {
            if (noun.Primary == "fish")
            {
                this.Output("This is no time to play cards.");
                return true;
            }

            return base.GoCore(noun);
        }

        private void Hello(Word verb, Word noun)
        {
            this.Output("Hello, " + noun + "!");
        }
    }
}
