using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Concordance
{
    public class ConcordanceGenerator
    {
        private readonly string _text;

        private readonly IList<ConcordanceStat> _stats;

        public ConcordanceGenerator(string text)
        {
            _stats = new List<ConcordanceStat>();
            _text = text ?? string.Empty;
            this.Generate();
        }

        public IEnumerable<ConcordanceStat> Stats => _stats;

        public override string ToString()
        {
            return string.Join(Environment.NewLine, _stats.OrderBy(x => x.Word));
        }

        private void Generate()
        {
            _stats.Clear();
            var sentences = this.SplitTextBySentence().ToList();
            for (int i = 0; i < sentences.Count; i++)
            {
                // Splits the words by whitespace, which is the default split option. Remove anything that is empty.
                var words = sentences[i].Split(new string[0], StringSplitOptions.RemoveEmptyEntries);
                foreach (var word in words)
                {
                    var wordAsAlpha = word.JustAlpha();
                    if (string.IsNullOrWhiteSpace(wordAsAlpha))
                    {
                        continue;
                    }
                    
                    // Get the stat for this word if it exists
                    var existingStat = _stats.Where(x => x.Matches(word)).FirstOrDefault();
                    if (existingStat == null)
                    {
                        // Create and add new stat
                        existingStat = new ConcordanceStat(word.JustAlpha());
                        _stats.Add(existingStat);
                    }
                    
                    // Add occurrence for the current sentence (example used 1-based indexing)
                    existingStat.AddOccurrence(i + 1);
                }
            }
        }

        // Splits text up into sentences, defined by either:
        // - a terminator followed by some whitespace and then an uppercase letter
        // - reaching the end of the string
        private IEnumerable<string> SplitTextBySentence()
        {
            var sentences = new List<string>();
            var currentSentence = string.Empty;
            for (int i = 0; i < _text.Length; i++)
            {
                currentSentence += _text[i];
                if (new [] { '.', '!', '?' }.Contains(_text[i]))
                {
                    var endOfSentenceFound = false;
                    var endOfTextFound = i == _text.Length - 1;
                    var whitespaceFound = false;
                    for (int j = i + 1; j < _text.Length; j++)
                    {
                        endOfTextFound = j == _text.Length - 1;
                        char c = _text[j];
                        if (char.IsWhiteSpace(c))
                        {
                            // Keep looking ahead as long as it is just whitespace
                            whitespaceFound = true;
                        }
                        else
                        {
                            // If the character is uppercase, we found the end
                            if (whitespaceFound && char.IsLetter(c) && char.IsUpper(c))
                            {
                                endOfSentenceFound = true;
                            }
                            break;
                        }
                    }

                    if (endOfSentenceFound || endOfTextFound)
                    {
                        sentences.Add(currentSentence.Trim());
                        currentSentence = string.Empty;
                    }
                }
                else if (i == _text.Length - 1)
                {
                    // End of sentence found without a terminator
                    sentences.Add(currentSentence.Trim());
                    currentSentence = string.Empty;
                }
            }

            return sentences;
        }
    }
}
