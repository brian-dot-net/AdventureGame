// <copyright file="MessageBus.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    using System;
    using System.Collections.Generic;

    public sealed class MessageBus
    {
        private readonly TypeMap<Action<object>> subscribers;

        public MessageBus()
        {
            this.subscribers = new TypeMap<Action<object>>();
        }

        public void Subscribe<TMessage>(Action<TMessage> subscriber)
        {
            this.subscribers[typeof(TMessage)] += o => subscriber((TMessage)o);
        }

        public void Send<TMessage>(TMessage message)
        {
            this.subscribers[typeof(TMessage)]?.Invoke(message);
        }

        private sealed class TypeMap<TValue>
        {
            private readonly Dictionary<Type, TValue> map;

            public TypeMap()
            {
                this.map = new Dictionary<Type, TValue>();
            }

            public TValue this[Type key]
            {
                get
                {
                    if (!this.map.ContainsKey(key))
                    {
                        this.map.Add(key, default(TValue));
                    }

                    return this.map[key];
                }

                set
                {
                    this.map[key] = value;
                }
            }
        }
    }
}
