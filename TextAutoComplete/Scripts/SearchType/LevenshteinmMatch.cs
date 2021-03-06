﻿using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Fastenshtein
{
    public class LevenshteinmMatch : MonoBehaviour
    {
        public string textpath;
        public string searchTerm;
        private List<string> termList;

        // Start is called before the first frame update
        void Start()
        {
            var stream = new StreamReader(textpath);
            termList = new List<string> {"armed", "armed", "jazz", "jaws"};

            while (!stream.EndOfStream)
                termList.Add(stream.ReadLine());

            Debug.Log("Characters added from file");

            SearchLev();
        }

        private void SearchLev()
        {
            Debug.Log("Searching Lev now");
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            Levenshtein lev = new Levenshtein(searchTerm);
            for (var index = 0; index < termList.Count; index++)
            {
                var term = termList[index];
                int levenshteinDistance = lev.DistanceFrom(term);
            }

            stopwatch.Stop();
            Debug.Log("Lev terms in ticks: " + stopwatch.ElapsedTicks);
            
            Debug.Log("Break");

        }
    }
}

namespace Fastenshtein
{
    public partial class Levenshtein
    {
        /*
         * WARRING this class is performance critical (Speed).
         */

        private readonly string storedValue;
        private readonly int[] costs;

        /// <summary>
        /// Creates a new instance with a value to test other values against
        /// </summary>
        /// <param Name="value">Value to compare other values to.</param>
        public Levenshtein(string value)
        {
            this.storedValue = value;
            // Create matrix row
            this.costs = new int[this.storedValue.Length];
        }

        /// <summary>
        /// gets the length of the stored value that is tested against
        /// </summary>
        public int StoredLength
        {
            get { return this.storedValue.Length; }
        }

        /// <summary>
        /// Compares a value to the stored value. 
        /// Not thread safe.
        /// </summary>
        /// <returns>Difference. 0 complete match.</returns>
        public int DistanceFrom(string value)
        {
            if (costs.Length == 0)
            {
                return value.Length;
            }

            // Add indexing for insertion to first row
            for (int i = 0; i < this.costs.Length;)
            {
                this.costs[i] = ++i;
            }

            for (int i = 0; i < value.Length; i++)
            {
                // cost of the first index
                int cost = i;
                int addationCost = i;

                // cache value for inner loop to avoid index lookup and bonds checking, profiled this is quicker
                char value1Char = value[i];

                for (int j = 0; j < this.storedValue.Length; j++)
                {
                    int insertionCost = cost;

                    cost = addationCost;

                    // assigning this here reduces the array reads we do, improvment of the old version
                    addationCost = this.costs[j];

                    if (value1Char != this.storedValue[j])
                    {
                        if (insertionCost < cost)
                        {
                            cost = insertionCost;
                        }

                        if (addationCost < cost)
                        {
                            cost = addationCost;
                        }

                        ++cost;
                    }

                    this.costs[j] = cost;
                }
            }

            return this.costs[this.costs.Length - 1];
        }
    }
}