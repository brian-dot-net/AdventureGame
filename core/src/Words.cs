﻿// <copyright file="Words.cs" company="Brian Rogers">
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
            this.words = new Dictionary<string, string>();
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

            if (this.words.ContainsKey(primary))
            {
                throw new InvalidOperationException($"Primary '{primary}' already exists.");
            }

            this.words.Add(primary, primary);
            foreach (string synonym in synonyms)
            {
                if (synonym == null)
                {
                    throw new ArgumentException($"Synonym for '{primary}' cannot be null.", nameof(synonyms));
                }

                if (this.words.ContainsKey(synonym))
                {
                    throw new InvalidOperationException($"Synonym '{synonym}' already exists.");
                }

                this.words.Add(synonym, primary);
            }
        }
    }
}