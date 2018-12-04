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
        private readonly Words words;

        public Game(TextReader reader, TextWriter writer)
        {
            this.bus = new MessageBus();
            this.console = new TextConsole(this.bus, reader, writer);
            this.words = InitializeWords();
        }

        public void Run()
        {
            using (CancellationTokenSource cts = new CancellationTokenSource())
            using (new SentenceParser(this.bus, this.words))
            using (InputLoop loop = this.console.NewLoop())
            {
                Room room = new MainRoom(this.bus, cts);
                room.Enter();
                loop.Run(cts.Token);
            }
        }

        private static Words InitializeWords()
        {
            Words w = new Words();
            w.Add(Verb.Greet, "hello", "hi");
            w.Add(Verb.Quit, "exit");
            w.Add(Verb.Take, "get");
            return w;
        }
    }
}
