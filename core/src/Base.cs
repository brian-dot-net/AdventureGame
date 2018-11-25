// <copyright file="Base.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    public abstract class Base
    {
        protected Base(string text)
        {
            this.Text = text;
        }

        public string Text { get; }
    }
}
