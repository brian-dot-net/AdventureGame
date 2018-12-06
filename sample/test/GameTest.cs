// <copyright file="GameTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Sample.Test
{
    using System.IO;
    using FluentAssertions;
    using Xunit;

    public sealed class GameTest
    {
        [Fact]
        public void WalkthroughTest()
        {
            const string ActualOut = "walkthrough.actual.out";
            const string ExpectedOut = "walkthrough.out";

            using (StreamReader reader = new StreamReader("walkthrough.in"))
            using (StreamWriter writer = new StreamWriter(ActualOut))
            {
                new Game().Run(reader, writer);
            }

            File.ReadAllLines(ActualOut).Should().Equal(File.ReadAllLines(ExpectedOut));
        }
    }
}
