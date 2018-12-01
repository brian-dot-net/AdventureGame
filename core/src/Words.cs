// <copyright file="Words.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    public sealed class Words
    {
        public Word this[string key]
        {
            get
            {
                return new Word(key, key);
            }
        }
    }
}
