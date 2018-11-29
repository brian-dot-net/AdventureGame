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
        [Fact]
        public void SendOneWord()
        {
            MessageBus bus = new MessageBus();
            List<string> sentences = new List<string>();
            Action<SentenceMessage> onSentence = m => sentences.Add(m.Verb + ":" + m.Noun);
            bus.Subscribe(onSentence);
            SentenceParser parser = new SentenceParser(bus);

            bus.Send(new InputMessage("one"));

            sentences.Should().ContainSingle().Which.Should().Be("one:");
        }

        [Fact]
        public void SendTwoWords()
        {
            MessageBus bus = new MessageBus();
            List<string> sentences = new List<string>();
            Action<SentenceMessage> onSentence = m => sentences.Add(m.Verb + ":" + m.Noun);
            bus.Subscribe(onSentence);
            SentenceParser parser = new SentenceParser(bus);

            bus.Send(new InputMessage("one two"));

            sentences.Should().ContainSingle().Which.Should().Be("one:two");
        }
    }
}
