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
        private readonly Words words;

        public Game(TextReader reader, TextWriter writer)
        {
            this.bus = new MessageBus();
            this.console = new TextConsole(this.bus, reader, writer);
            this.words = InitializeWords();
        }

        public void Run()
        {
            using (new SentenceParser(this.bus, this.words))
            using (QuitHandler quit = new QuitHandler(this.bus, Verb.Quit))
            using (InputLoop loop = this.console.NewLoop())
            {
                Room room = new MainRoom(this.bus);
                room.Enter();
                loop.Run(quit.Token);
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
