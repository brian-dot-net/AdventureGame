// <copyright file="WordsTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Test
{
    using System;
    using FluentAssertions;
    using Xunit;

    public sealed class WordsTest
    {
        [Fact]
        public void AddNullPrimary()
        {
            Words words = new Words();

            Action act = () => words.Add(null);

            act.Should().Throw<ArgumentNullException>().Which.ParamName.Should().Be("primary");
        }

        [Fact]
        public void AddNullSynonym()
        {
            Words words = new Words();

            Action act = () => words.Add("x", "y", null);

            act.Should().Throw<ArgumentException>().WithMessage("*'x'*").Which.ParamName.Should().Be("synonyms");
            words["x"].Primary.Should().Be("x");
            words["y"].Primary.Should().Be("x");
        }

        [Fact]
        public void AddDuplicateSynonym()
        {
            Words words = new Words();

            Action act = () => words.Add("x", "y", "y");

            act.Should().Throw<InvalidOperationException>().WithMessage("Synonym 'y' already exists.");
            words["y"].Primary.Should().Be("x");
        }

        [Fact]
        public void AddDuplicatePrimary()
        {
            Words words = new Words();

            words.Add("x", "y");
            Action act = () => words.Add("x", "z");

            act.Should().Throw<InvalidOperationException>().WithMessage("Primary 'x' already exists.");
            words["z"].Primary.Should().BeEmpty();
        }

        [Fact]
        public void CaseInsensitiveMatching()
        {
            Words words = new Words();
            words.Add("Hello", "hi", "HOWDY");

            Word w1 = words["Hi"];
            Word w2 = words["howdY"];
            Word w3 = words["hello"];

            w1.Should().BeEquivalentTo(new Word("Hello", "Hi"));
            w2.Should().BeEquivalentTo(new Word("Hello", "howdY"));
            w3.Should().BeEquivalentTo(new Word("Hello", "hello"));
        }
    }
}
