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
            using (CancellationTokenSource cts = new CancellationTokenSource())
            using (new SentenceParser(this.bus, new Words()))
            using (this.bus.Subscribe<SentenceMessage>(m => this.ProcessVerb(cts, m.Verb)))
            {
                this.console.Run(cts.Token);
            }
        }

        private void ProcessVerb(CancellationTokenSource cts, Word verb)
        {
            string output = null;
            if (verb.Actual == "hello")
            {
                output = "world";
            }
            else if (verb.Actual == "quit")
            {
                cts.Cancel();
            }
            else
            {
                output = "I don't know what '" + verb + "' means.";
            }

            if (output != null)
            {
                this.bus.Send(new OutputMessage(output));
            }
        }
    }
}
