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
        private readonly IDisposable sub;

        public QuitHandler(MessageBus bus, string verb)
        {
            this.cts = new CancellationTokenSource();
            this.sub = bus.Subscribe<SentenceMessage>(m => this.Handle(m.Verb.Primary == verb));
        }

        public CancellationToken Token => this.cts.Token;

        public void Dispose()
        {
            using (this.cts)
            using (this.sub)
            {
            }
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
