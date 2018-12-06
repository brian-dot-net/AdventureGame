// <copyright file="OldInputLoop.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    using System;
    using System.Threading;

    public sealed class OldInputLoop : IDisposable
    {
        private readonly Func<bool> readInput;
        private readonly IDisposable sub;

        public OldInputLoop(MessageBus bus, Func<bool> readInput, Action<OutputMessage> onOutput)
        {
            this.readInput = readInput;
            this.sub = bus.Subscribe(onOutput);
        }

        public void Run(CancellationToken token)
        {
            try
            {
                this.RunInner(token);
            }
            catch (OperationCanceledException)
            {
            }
        }

        public void Dispose()
        {
            this.sub.Dispose();
        }

        private void RunInner(CancellationToken token)
        {
            do
            {
                token.ThrowIfCancellationRequested();
            }
            while (this.readInput());
        }
    }
}
