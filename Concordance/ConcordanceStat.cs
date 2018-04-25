using System.Collections.Generic;

namespace Concordance
{
    public class ConcordanceStat
    {
        private readonly IList<int> _sentenceIndices;

        public ConcordanceStat(string word)
        {
            this.Word = word;
            _sentenceIndices = new List<int>();
        }

        public string Word { get; }
        public int Frequency { get; private set; }
        public IEnumerable<int> SentenceIndices => _sentenceIndices;

        public void AddOccurrence(int sentenceIndex)
        {
            this.Frequency++;
            _sentenceIndices.Add(sentenceIndex);
        }

        public bool Matches(string word)
        {
            return this.Word.JustAlpha() == word.JustAlpha();
        }

        public override string ToString()
        {
            return $"{this.Word} {{{this.Frequency}:{string.Join(",", this.SentenceIndices)}}}";
        }
    }
}
