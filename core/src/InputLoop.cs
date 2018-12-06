// <copyright file="InputLoop.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    using System;
    using System.Threading;

    public sealed class InputLoop : IDisposable
    {
        private readonly MessageBus bus;
        private readonly IDisposable sub;

        private bool inputEnded;

        public InputLoop(MessageBus bus)
        {
            this.bus = bus;
            this.sub = bus.Subscribe<InputEndedMessage>(_ => this.inputEnded = true);
        }

        public void Run(CancellationToken token)
        {
            while (!this.inputEnded && !token.IsCancellationRequested)
            {
                this.bus.Send(new InputRequestedMessage());
            }
        }

        public void Dispose()
        {
            this.sub.Dispose();
        }
    }
}
