// <copyright file="DerivedTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.App.Test
{
    using FluentAssertions;
    using Xunit;

    public class DerivedTest
    {
        [Fact]
        public void Test1()
        {
            new Derived("def").Text.Should().Be("def");
        }
    }
}
