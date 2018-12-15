// <copyright file="MessageBusExtensions.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Messages
{
    public static class MessageBusExtensions
    {
        public static void Output(this MessageBus bus, string text)
        {
            bus.Send(new OutputMessage(text));
        }
    }
}
