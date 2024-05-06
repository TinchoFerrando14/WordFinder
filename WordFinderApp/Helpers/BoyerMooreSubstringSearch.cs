using System;
using System.Collections.Generic;

namespace WordFinderApp.Helpers
{
    public static class BoyerMooreSubstringSearch
    {
        public static List<int> Search(string text, string pattern)
        {
            var occurrences = new List<int>();

            int textLength = text.Length;
            int patternLength = pattern.Length;

            var last = BuildLast(pattern);

            int i = patternLength - 1;
            int j = patternLength - 1;

            while (i < textLength)
            {
                if (text[i] == pattern[j])
                {
                    if (j == 0)
                    {
                        occurrences.Add(i);
                        i += patternLength; // Jump to next potential match
                        j = patternLength - 1;
                    }
                    else
                    {
                        i--;
                        j--;
                    }
                }
                else
                {
                    i += patternLength - Math.Min(j, 1 + last[text[i]]);
                    j = patternLength - 1;
                }
            }

            return occurrences;
        }

        public static List<(int Row, int Column)> SearchInList(List<string> list, string pattern)
        {
            var occurrences = new List<(int Row, int Column)>();

            for (int i = 0; i < list.Count; i++)
            {
                List<int> foundIndexes = Search(list[i], pattern);
                foreach (int index in foundIndexes)
                {
                    occurrences.Add((i, index));
                }
            }

            return occurrences;
        }

        private static int[] BuildLast(string pattern)
        {
            const int R = 256; // ASCII characters
            var last = new int[R];

            for (int i = 0; i < R; i++)
            {
                last[i] = -1;
            }

            for (int i = 0; i < pattern.Length; i++)
            {
                last[pattern[i]] = i;
            }

            return last;
        }
    }
}
