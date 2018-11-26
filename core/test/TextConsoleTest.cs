// <copyright file="TextConsoleTest.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Test
{
    using System;
    using System.IO;
    using FluentAssertions;
    using Xunit;

    public class TextConsoleTest
    {
        [Fact]
        public void RunNoInput()
        {
            TextConsole con = new TextConsole(new MessageBus(), TextReader.Null, TextWriter.Null);

            Action act = () => con.Run();

            act.Should().NotThrow();
        }
    }
}
