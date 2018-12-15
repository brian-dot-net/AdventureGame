// <copyright file="InputLoop.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    using System;
    using System.Threading;
    using Adventure.Messages;

    public sealed class InputLoop : IDisposable
    {
        private readonly MessageBus bus;
        private readonly IDisposable sub;
        private readonly string prompt;

        private bool inputEnded;

        public InputLoop(MessageBus bus, string prompt = null)
        {
            this.bus = bus;
            this.sub = bus.Subscribe<InputEndedMessage>(_ => this.inputEnded = true);
            this.prompt = prompt;
        }

        public void Run(CancellationToken token)
        {
            while (!this.inputEnded && !token.IsCancellationRequested)
            {
                this.bus.Send(new InputRequestedMessage(this.prompt));
            }
        }

        public void Dispose()
        {
            this.sub.Dispose();
        }
    }
}
