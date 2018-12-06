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

        public InputLoop(MessageBus bus)
        {
            this.bus = bus;
        }

        public void Run(CancellationToken token)
        {
        }

        public void Dispose()
        {
        }
    }
}
