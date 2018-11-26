// <copyright file="MessageBus.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    using System;
    using System.Collections.Generic;

    public sealed class MessageBus
    {
        private readonly TypeMap<Subscribers> subscribers;

        public MessageBus()
        {
            this.subscribers = new TypeMap<Subscribers>();
        }

        public IDisposable Subscribe<TMessage>(Action<TMessage> subscriber)
        {
            return this.subscribers[typeof(TMessage)].Add(subscriber);
        }

        public void Send<TMessage>(TMessage message)
        {
            this.subscribers[typeof(TMessage)].Invoke(message);
        }

        private sealed class Subscribers
        {
            private Action<object> subscribers;

            public IDisposable Add<TMessage>(Action<TMessage> subscriber)
            {
                Action<object> next = o => subscriber((TMessage)o);
                this.subscribers += next;
                return new Subscription(this, next);
            }

            public void Invoke<TMessage>(TMessage message)
            {
                this.subscribers?.Invoke(message);
            }

            private sealed class Subscription : IDisposable
            {
                private readonly Subscribers parent;
                private readonly Action<object> self;

                public Subscription(Subscribers parent, Action<object> self)
                {
                    this.parent = parent;
                    this.self = self;
                }

                public void Dispose()
                {
                    this.parent.subscribers -= this.self;
                }
            }
        }

        private sealed class TypeMap<TValue>
            where TValue : new()
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
                        this.map.Add(key, new TValue());
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
