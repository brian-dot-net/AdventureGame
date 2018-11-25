// <copyright file="BaseTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Test
{
    using FluentAssertions;
    using Xunit;

    public class BaseTest
    {
        [Fact]
        public void Test1()
        {
            new TestBase("abc").Text.Should().Be("abc");
        }

        private sealed class TestBase : Base
        {
            public TestBase(string text)
                : base(text)
            {
            }
        }
    }
}
