// <copyright file="QuitHandler.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    using System;
    using System.Threading;

    public sealed class QuitHandler : IDisposable
    {
        private readonly CancellationTokenSource cts;

        public QuitHandler(MessageBus bus, string verb)
        {
            this.cts = new CancellationTokenSource();
            bus.Subscribe<SentenceMessage>(m => this.Handle(m.Verb.Primary == verb));
        }

        public CancellationToken Token => this.cts.Token;

        public void Dispose()
        {
        }

        private bool Handle(bool shouldQuit)
        {
            if (shouldQuit)
            {
                this.cts.Cancel();
            }

            return shouldQuit;
        }
    }
}
