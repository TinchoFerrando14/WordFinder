using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Concurrent;
using WordFinderApp.Helpers;

namespace WordFinderApp
{
    public class WordFinder
    {
        private readonly List<string> _horizontalWords;
        private readonly List<string> _verticalWords;
        private readonly object _lockObject = new();
        private SortedSet<(int ocurrencies, string word)> _results = new();

        private const int MaxWordsToReturn = 10;


        public WordFinder(IEnumerable<string> matrix)
        {
            _horizontalWords = matrix.ToList();
            _verticalWords = GetTransposedMatrix(matrix);
        }

        public IEnumerable<string> Find(IEnumerable<string> wordstream)
        {
            var processedWords = new ConcurrentDictionary<string, bool>();

            Parallel.ForEach(wordstream, word =>
            {
                if (processedWords.TryAdd(word, true))
                {

                    var horizontalOccurrences = BoyerMooreSubstringSearch.SearchInList(_horizontalWords, word).Count;
                    var verticalOccurrences = BoyerMooreSubstringSearch.SearchInList(_verticalWords, word).Count;

                    var totalOcurrencies = horizontalOccurrences + verticalOccurrences;

                    if (totalOcurrencies > 0)
                    {
                        AddWordToSortedSetConcurrent(word, horizontalOccurrences + verticalOccurrences);
                    }
                }
            });

            return _results.ToList().Select(result => result.word); 
        }

        private void AddWordToSortedSetConcurrent(string word, int ocurrences)
        {
            lock (_lockObject)
            {
                _results.Add((ocurrences, word));

                if (_results.Count > MaxWordsToReturn)
                {
                    _results.Remove(_results.First());
                }
            }
        }

        private static List<string> GetTransposedMatrix(IEnumerable<string> matrix)
        {
            var transposedMatrix = new List<string>();

            if (!matrix.Any())
            {
                return transposedMatrix; 
            }

            int numRows = matrix.Count();
            int numCols = matrix.ElementAt(0).Length;

            Parallel.For(0, numCols, j =>
            {
                var transposedRow = new StringBuilder(numRows);
                for (int i = 0; i < numRows; i++)
                {
                    transposedRow.Append(matrix.ElementAt(i)[j]);
                }
                lock (transposedMatrix)
                {
                    transposedMatrix.Add(transposedRow.ToString());
                }
            });

            return transposedMatrix;
        }
    }

   
}