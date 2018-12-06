// <copyright file="Program.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure.Sample
{
    using System;

    internal static class Program
    {
        private static void Main()
        {
            new Game().Run(Console.In, Console.Out);
        }
    }
}
