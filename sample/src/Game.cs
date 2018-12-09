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
            using (InputLoop loop = new InputLoop(this.bus, ">"))
            using (RoomMap map = this.InitializeMap())
            {
                loop.Run(quit.Token);
            }
        }

        private static Words InitializeWords()
        {
            Words w = new Words();
            w.Add(Verb.Greet, "hello", "hi");
            w.Add(Verb.Look);
            w.Add(Verb.Quit, "exit");
            w.Add(Verb.Take, "get");

            w.Add(Noun.Table);

            return w;
        }

        private RoomMap InitializeMap()
        {
            RoomMap map = new RoomMap(this.bus);

            MainRoom mainRoom = new MainRoom(this.bus);
            var mainRoomP = map.Add(mainRoom);

            map.Start(mainRoomP);

            return map;
        }
    }
}
