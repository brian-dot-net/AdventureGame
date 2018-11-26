// <copyright file="MessageBus.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    using System;
    using System.Collections.Generic;

    public sealed class MessageBus
    {
        private readonly Dictionary<Type, Action<object>> subscribers;

        public MessageBus()
        {
            this.subscribers = new Dictionary<Type, Action<object>>();
        }

        public void Subscribe<TMessage>(Action<TMessage> subscriber)
        {
            Type key = typeof(TMessage);
            if (!this.subscribers.ContainsKey(key))
            {
                this.subscribers.Add(key, null);
            }

            this.subscribers[key] += o => subscriber((TMessage)o);
        }

        public void Send<TMessage>(TMessage message)
        {
            if (this.subscribers.TryGetValue(typeof(TMessage), out Action<object> value))
            {
                value(message);
            }
        }
    }
}
