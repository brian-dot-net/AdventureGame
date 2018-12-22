// <copyright file="ActionMessage.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Messages
{
    using System;

    public sealed class ActionMessage<T>
    {
        public ActionMessage(Action<T> act)
        {
            this.Act = act;
        }

        public Action<T> Act { get; }
    }
}
