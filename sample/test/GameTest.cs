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
        public void WalkthroughQuitTest()
        {
            const string ActualOut = "walkthrough.quit.actual.out";
            const string ExpectedOut = "walkthrough.quit.out";

            using (StreamReader reader = new StreamReader("walkthrough.quit.in"))
            using (StreamWriter writer = new StreamWriter(ActualOut))
            {
                new Game().Run(reader, writer);
            }

            File.ReadAllLines(ActualOut).Should().Equal(File.ReadAllLines(ExpectedOut));
        }

        [Fact]
        public void WalkthroughDieTest()
        {
            const string ActualOut = "walkthrough.die.actual.out";
            const string ExpectedOut = "walkthrough.die.out";

            using (StreamReader reader = new StreamReader("walkthrough.die.in"))
            using (StreamWriter writer = new StreamWriter(ActualOut))
            {
                new Game().Run(reader, writer);
            }

            File.ReadAllLines(ActualOut).Should().Equal(File.ReadAllLines(ExpectedOut));
        }
    }
}
