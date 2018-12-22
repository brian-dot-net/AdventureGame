// <copyright file="InventoryActionMessage.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Messages
{
    using System;

    public sealed class InventoryActionMessage
    {
        public InventoryActionMessage(Action<Inventory> act)
        {
            this.Act = act;
        }

        public Action<Inventory> Act { get; }
    }
}
