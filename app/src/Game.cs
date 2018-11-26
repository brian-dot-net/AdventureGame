// <copyright file="Game.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.App
{
    using System.IO;

    public sealed class Game
    {
        private readonly MessageBus bus;
        private readonly TextConsole console;

        public Game(MessageBus bus, TextReader reader, TextWriter writer)
        {
            this.bus = bus;
            this.console = new TextConsole(bus, reader, writer);
        }

        public void Run()
        {
            using (this.bus.Subscribe<InputMessage>(m => this.ProcessInput(m.Line)))
            {
                this.console.Run();
            }
        }

        private void ProcessInput(string line)
        {
            if (line == "hello")
            {
                this.bus.Send(new OutputMessage("world"));
            }
        }
    }
}
