// <copyright file="EndOfGame.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    using System;
    using System.Threading;
    using Adventure.Messages;

    public sealed class EndOfGame : IDisposable
    {
        private readonly CancellationTokenSource cts;
        private readonly IDisposable sub;

        public EndOfGame(MessageBus bus)
        {
            this.cts = new CancellationTokenSource();
            this.sub = bus.Subscribe<EndOfGameMessage>(m => this.OnEnd(bus, m.Text));
        }

        public CancellationToken Token => this.cts.Token;

        public void Dispose()
        {
        }

        private void OnEnd(MessageBus bus, string text)
        {
            bus.Output(text);
            this.cts.Cancel();
        }
    }
}
