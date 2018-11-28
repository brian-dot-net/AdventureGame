// <copyright file="Game.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Sample
{
    using System.IO;

    public sealed class Game
    {
        private readonly MessageBus bus;
        private readonly TextConsole console;

        public Game(TextReader reader, TextWriter writer)
        {
            this.bus = new MessageBus();
            this.console = new TextConsole(this.bus, reader, writer);
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
