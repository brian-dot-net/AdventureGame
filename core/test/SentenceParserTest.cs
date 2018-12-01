// <copyright file="SentenceParserTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Test
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using Xunit;

    public class SentenceParserTest
    {
        [Theory]
        [InlineData("one", "one:")]
        [InlineData("one ", "one:")]
        [InlineData(" one ", "one:")]
        [InlineData("one two", "one:two")]
        [InlineData("one  two", "one:two")]
        [InlineData(" one two", "one:two")]
        [InlineData("one two ", "one:two")]
        public void SendWords(string input, string output)
        {
            MessageBus bus = new MessageBus();
            List<string> sentences = new List<string>();
            Action<SentenceMessage> onSentence = m => sentences.Add(m.Verb + ":" + m.Noun);
            bus.Subscribe(onSentence);
            SentenceParser parser = new SentenceParser(bus, new Words());

            bus.Send(new InputMessage(input));

            sentences.Should().ContainSingle().Which.Should().Be(output);
        }

        [Fact]
        public void SendAfterDispose()
        {
            MessageBus bus = new MessageBus();
            List<string> sentences = new List<string>();
            Action<SentenceMessage> onSentence = m => sentences.Add(m.Verb + ":" + m.Noun);
            bus.Subscribe(onSentence);
            SentenceParser parser = new SentenceParser(bus, new Words());

            parser.Dispose();
            bus.Send(new InputMessage("hello"));

            sentences.Should().BeEmpty();
        }

        [Theory]
        [InlineData("see home", "look|house")]
        [InlineData("inspect house", "look|house")]
        [InlineData("examine home", "look|house")]
        [InlineData("look house", "look|house")]
        [InlineData("watch building", "|")]
        public void SendWordsSynonyms(string input, string output)
        {
            MessageBus bus = new MessageBus();
            List<string> sentences = new List<string>();
            Action<SentenceMessage> onSentence = m => sentences.Add(m.Verb.Primary + "|" + m.Noun.Primary);
            bus.Subscribe(onSentence);
            Words words = new Words();
            words.Add("look", "see", "inspect", "examine");
            words.Add("house", "home");
            SentenceParser parser = new SentenceParser(bus, words);

            bus.Send(new InputMessage(input));

            sentences.Should().ContainSingle().Which.Should().Be(output);
        }
    }
}
