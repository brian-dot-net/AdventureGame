// <copyright file="Inventory.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    using System;

    public sealed class Inventory : IDisposable
    {
        private readonly MessageBus bus;
        private readonly IDisposable sub;

        public Inventory(MessageBus bus)
        {
            this.bus = bus;
            this.sub = bus.Subscribe<InventoryRequestedMessage>(m => this.Show());
        }

        public void Dispose()
        {
        }

        private void Show()
        {
            this.Output("You are carrying:");
            this.Output("(nothing)");
        }

        private void Output(string text)
        {
            this.bus.Send(new OutputMessage(text));
        }
    }
}
