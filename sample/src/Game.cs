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
            Words words = new Words();
            words.Add("greet", "hello", "hi");
            words.Add("quit", "exit");
            words.Add("take", "get");
            using (CancellationTokenSource cts = new CancellationTokenSource())
            using (new SentenceParser(this.bus, words))
            using (this.bus.Subscribe<SentenceMessage>(m => this.ProcessSentence(cts, m)))
            {
                this.console.Run(cts.Token);
            }
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
