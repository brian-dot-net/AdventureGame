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
            using (EndOfGame endOfGame = new EndOfGame(this.bus))
            using (InputLoop loop = new InputLoop(this.bus, ">"))
            using (new Inventory(this.bus))
            using (this.InitializeMap())
            {
                loop.Run(endOfGame.Token);
            }
        }

        private static Words InitializeWords()
        {
            Words w = new Words();

            Verb.Register(w);
            Noun.Register(w);

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
