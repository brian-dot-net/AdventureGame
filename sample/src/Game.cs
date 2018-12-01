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
            string output = null;
            Word verb = sentence.Verb;
            if (verb.Primary == "greet")
            {
                output = "You say, \"Hello,\" to no one in particular. No one answers.";
            }
            else if (verb.Primary == "quit")
            {
                cts.Cancel();
            }
            else if (verb.Primary == "take")
            {
                output = "There is no " + sentence.Noun.Actual.ToLowerInvariant() + " here.";
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
