// <copyright file="MainRoom.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Sample
{
    using System.Threading;

    internal sealed class MainRoom : Room
    {
        private readonly CancellationTokenSource cts;

        public MainRoom(MessageBus bus, CancellationTokenSource cts)
            : base(bus)
        {
            this.cts = cts;
        }

        protected override void EnterCore()
        {
            this.Register(Verb.Greet, (_, __) => this.Output("You say, \"Hello,\" to no one in particular. No one answers."));
            this.Register(Verb.Quit, (_, n) => this.cts.Cancel());
            this.Register(Verb.Take, (_, n) => this.Output("There is no " + n.Actual.ToLowerInvariant() + " here."));
        }
    }
}
