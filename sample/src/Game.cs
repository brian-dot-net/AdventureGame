// <copyright file="Game.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Sample
{
    using System.IO;
    using System.Threading;

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
                this.console.Run(CancellationToken.None);
            }
        }

        private void ProcessVerb(string verb)
        {
            string output = null;
            if (verb == "hello")
            {
                output = "world";
            }
            else
            {
                output = "I don't know what '" + verb + "' means.";
            }

            this.bus.Send(new OutputMessage(output));
        }
    }
}
