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
            using (new SentenceParser(this.bus))
            using (this.bus.Subscribe<SentenceMessage>(m => this.ProcessVerb(m.Verb)))
            {
                this.console.Run();
            }
        }

        private void ProcessVerb(string verb)
        {
            if (verb == "hello")
            {
                this.bus.Send(new OutputMessage("world"));
            }
        }
    }
}
