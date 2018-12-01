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
    }
}
