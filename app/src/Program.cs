// <copyright file="Program.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.App
{
    using System;

    internal static class Program
    {
        private static void Main()
        {
            new Game(Console.In, Console.Out).Run();
        }
    }
}
