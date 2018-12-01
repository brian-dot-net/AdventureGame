// <copyright file="Words.cs" company="Brian Rogers">
// Copyright (c) Brian Rogers. All rights reserved.
// </copyright>

namespace Adventure
{
    using System;
    using System.Collections.Generic;

    public sealed class Words
    {
        private readonly Dictionary<string, string> words;

        public Words()
        {
            this.words = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        public Word this[string actual]
        {
            get
            {
                if (!this.words.TryGetValue(actual, out string primary))
                {
                    primary = string.Empty;
                }

                return new Word(primary, actual);
            }
        }

        public void Add(string primary, params string[] synonyms)
        {
            if (primary == null)
            {
                throw new ArgumentNullException(nameof(primary));
            }

            this.Add("Primary", primary, primary);

            foreach (string synonym in synonyms)
            {
                if (synonym == null)
                {
                    throw new ArgumentException($"Synonym for '{primary}' cannot be null.", nameof(synonyms));
                }

                this.Add("Synonym", synonym, primary);
            }
        }

        private void Add(string kind, string key, string value)
        {
            if (this.words.ContainsKey(key))
            {
                throw new InvalidOperationException($"{kind} '{key}' already exists.");
            }

            this.words.Add(key, value);
        }
    }
}
