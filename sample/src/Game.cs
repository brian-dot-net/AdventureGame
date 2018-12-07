// <copyright file="Game.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Sample
{
    using System.IO;

    public sealed class Game
    {
        private readonly MessageBus bus;
        private readonly Words words;

        public Game()
        {
            this.bus = new MessageBus();
            this.words = InitializeWords();
        }

        public void Run(TextReader reader, TextWriter writer)
        {
            using (TextConsole console = new TextConsole(this.bus, reader, writer))
            using (new SentenceParser(this.bus, this.words))
            using (QuitHandler quit = new QuitHandler(this.bus, Verb.Quit))
            using (InputLoop loop = new InputLoop(this.bus))
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
