// <copyright file="Game.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.App
{
    using System.IO;

    public sealed class Game
    {
        private readonly TextReader reader;
        private readonly TextWriter writer;

        public Game(TextReader reader, TextWriter writer)
        {
            this.reader = reader;
            this.writer = writer;
        }

        public void Run()
        {
            string line;
            do
            {
                line = this.reader.ReadLine();
                if (line == "hello")
                {
                    this.writer.WriteLine("world");
                }
            }
            while (line != null);
        }
    }
}
