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
            using (this.bus.Subscribe<SentenceMessage>(m => this.ProcessSentence(cts, m)))
            {
                this.console.Run(cts.Token);
            }
        }

        private static Words InitializeWords()
        {
            Words w = new Words();
            w.Add("greet", "hello", "hi");
            w.Add("quit", "exit");
            w.Add("take", "get");
            return w;
        }

        private void ProcessSentence(CancellationTokenSource cts, SentenceMessage sentence)
        {
            string output = this.Process(cts, sentence.Verb, sentence.Noun);
            if (output != null)
            {
                this.bus.Send(new OutputMessage(output));
            }
        }

        private string Process(CancellationTokenSource cts, Word verb, Word noun)
        {
            switch (verb.Primary)
            {
                case "greet":
                    return "You say, \"Hello,\" to no one in particular. No one answers.";
                case "take":
                    return "There is no " + noun.Actual.ToLowerInvariant() + " here.";
                case "quit":
                    cts.Cancel();
                    return null;
                default:
                    return "I don't know what '" + verb + "' means.";
            }
        }
    }
}
