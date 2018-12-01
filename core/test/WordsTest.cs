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
    }
}
