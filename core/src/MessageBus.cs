// <copyright file="MessageBus.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    using System;

    public sealed class MessageBus
    {
        private Action<object> subscriber;

        public void Subscribe<TMessage>(Action<TMessage> subscriber)
        {
            this.subscriber = o => subscriber((TMessage)o);
        }

        public void Send<TMessage>(TMessage message)
        {
            this.subscriber?.Invoke(message);
        }
    }
}
