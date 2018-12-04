﻿// <copyright file="TextConsole.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    using System;
    using System.IO;
    using System.Threading;

    public sealed class TextConsole
    {
        private readonly MessageBus bus;
        private readonly TextReader reader;
        private readonly TextWriter writer;

        public TextConsole(MessageBus bus, TextReader reader, TextWriter writer)
        {
            this.bus = bus;
            this.reader = reader;
            this.writer = writer;
        }

        public void Run(CancellationToken token)
        {
            using (InputLoop loop = new InputLoop(this.bus, this.ReadLine, m => this.writer.WriteLine(m.Text)))
            {
                loop.Run(token);
            }
        }

        private bool ReadLine()
        {
            string line = this.reader.ReadLine();
            if (line == null)
            {
                return false;
            }

            this.bus.Send(new InputMessage(line));
            return true;
        }

        private sealed class InputLoop : IDisposable
        {
            private readonly Func<bool> readInput;
            private readonly IDisposable sub;

            public InputLoop(MessageBus bus, Func<bool> readInput, Action<OutputMessage> onOutput)
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
}
