using System;
using System.Linq;
using Xunit;

namespace Concordance.Tests
{
    public class ConcordanceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("123. $!, @#.")]
        public void NullOrNoLettersResultInEmptyConcordance(string input)
        {
            var gen = new ConcordanceGenerator(input);
            Assert.Equal(string.Empty, gen.ToString());
        }

        [Fact]
        public void SentenceWithSingleWordResultsInOneConcordanceStat()
        {
            var gen = new ConcordanceGenerator("Word.");
            Assert.Single(gen.Stats);
        }

        [Fact]
        public void FrequencyForOneWordIsOne()
        {
            var gen = new ConcordanceGenerator("Word.");
            Assert.Single(gen.Stats);
            Assert.Equal(1, gen.Stats.First().Frequency);
        }

        [Fact]
        public void SentenceIndexForOneWordIsOne()
        {
            var gen = new ConcordanceGenerator("Word.");
            Assert.Single(gen.Stats);
            Assert.Single(gen.Stats.First().SentenceIndices);
            Assert.Equal(1, gen.Stats.First().SentenceIndices.First());
        }

        [Fact]
        public void ConcordanceContainsLowerCasedWordWithoutPunctuation()
        {
            var gen = new ConcordanceGenerator("I.E.");
            Assert.Single(gen.Stats);
            Assert.Equal("ie", gen.Stats.First().Word);
        }

        [Theory]
        [InlineData("Say this, say.")]
        [InlineData("Yo, I'm Yo Yo!")]
        public void SentenceWithTwoUniqueWordsResultsInTwoConcordanceStats(string input)
        {
            var gen = new ConcordanceGenerator(input);
            Assert.Equal(2, gen.Stats.Count());
        }

        [Theory]
        [InlineData("Hi, I'm Evan. This is cool!")]
        [InlineData("Hi! Exclamations are terminators too.")]
        [InlineData("What's that you say? I'm currently speaking my second sentence?")]
        public void SecondSentenceIsStartedAfterAnyTerminator(string input)
        {
            var gen = new ConcordanceGenerator(input);
            Assert.Contains(gen.Stats, x => x.SentenceIndices.Contains(1));
            Assert.Contains(gen.Stats, x => x.SentenceIndices.Contains(2));
        }

        [Theory]
        [InlineData("This is fun. This is cool.")]
        [InlineData("Hi my name is Evan. Hi, what's your name?")]
        [InlineData("Tick. Tock. Tick.")]
        public void OccurrenceFoundInTwoSentencesHasBothIndices(string input)
        {
            var gen = new ConcordanceGenerator(input);
            Assert.Contains(gen.Stats, x => x.SentenceIndices.Count() == 2);
        }
    }
}
