using System;
using System.Collections.Generic;

namespace WordFinderApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var matrix = new List<string>
            {
                "aabcdcscsill",
                "afgwioscsill",
                "achillschill",
                "apqnsdasefgs",
                "auvdxyssdwfg"
            };

            var wordFinder = new WordFinder(matrix);

            var wordStream = new List<string>
            {
                "chill",
                "wind",
                "cold",
                "snow"
            };

            var result = wordFinder.Find(wordStream);
        }
    }
}
