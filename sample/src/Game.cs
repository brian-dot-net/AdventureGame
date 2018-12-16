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
            using (new TextConsole(this.bus, reader, writer))
            using (new SentenceParser(this.bus, this.words))
            using (QuitHandler quit = new QuitHandler(this.bus, Verb.Quit))
            using (InputLoop loop = new InputLoop(this.bus, ">"))
            using (new Inventory(this.bus))
            using (this.InitializeMap())
            {
                loop.Run(quit.Token);
            }
        }

        private static Words InitializeWords()
        {
            Words w = new Words();
            w.Add(Verb.Drop, "throw");
            w.Add(Verb.Go);
            w.Add(Verb.Greet, "hello", "hi");
            w.Add(Verb.Look);
            w.Add(Verb.Move);
            w.Add(Verb.Quit, "exit");
            w.Add(Verb.Read);
            w.Add(Verb.Take, "get");
            w.Add(Verb.Inventory, "inv");

            w.Add(Noun.Coin);
            w.Add(Noun.East);
            w.Add(Noun.Table);
            w.Add(Noun.West);

            return w;
        }

        private RoomMap InitializeMap()
        {
            RoomMap map = new RoomMap(this.bus);

            Room mainRoom = new MainRoom(this.bus);
            var mainRoomP = map.Add(mainRoom);

            Room auxiliaryRoom = new AuxiliaryRoom(this.bus);
            var auxiliaryRoomP = map.Add(auxiliaryRoom);

            mainRoomP.ConnectTo(auxiliaryRoomP, Noun.East);
            auxiliaryRoomP.ConnectTo(mainRoomP, Noun.West);

            map.Start(mainRoomP);

            return map;
        }
    }
}
