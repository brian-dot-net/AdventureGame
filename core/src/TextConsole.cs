﻿// <copyright file="TextConsole.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    using System;
    using System.IO;

    public sealed class TextConsole : IDisposable
    {
        private readonly MessageBus bus;

        public TextConsole(MessageBus bus, TextReader reader)
        {
            this.bus = bus;
            this.bus.Subscribe<InputRequestedMessage>(_ => this.ReadLine(reader));
        }

        public void Dispose()
        {
        }

        private void ReadLine(TextReader reader)
        {
            if (reader.ReadLine() == null)
            {
                this.bus.Send(new InputEndedMessage());
            }
        }
    }
}
