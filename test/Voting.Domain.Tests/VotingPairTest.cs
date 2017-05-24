using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Voting.Domain.Tests
{
    public class VotingPairTest
    {
        [Fact]
        public void Given_CreatedVotingPair_When_VoteForTopic_Then_NewVotingPair()
        {
            var result = VotingPair.Create("C#", "F#").VoteForTopic("C#");
            Assert.Equal(result.TopicA, ("C#", 1));
            Assert.Equal(result.TopicB, ("F#", 0));
        }

        [Fact]
        public void Given_CreatedVotingPair_When_VotingForOne_Then_OneWinner()
        {
            var result = VotingPair.Create("C#", "F#").VoteForTopic("C#").GetWinners();
            Assert.Equal(result.Count(), 1);
            Assert.Equal(result.First(), "C#");
        }

        [Fact]
        public void Given_CreatedVotingPair_When_VotingForAll_Then_AllWinners()
        {
            var result = VotingPair.Create("C#", "F#")
                .VoteForTopic("C#")
                .VoteForTopic("F#")
                .GetWinners();
            Assert.Equal(result.Count(), 2);
            Assert.Equal(result.First(), "C#");
            Assert.Equal(result.Last(), "F#");
        }

        [Fact]
        public void Given_CreatedVotingPair_When_MoreThanTwoTopics_Then_Exception()
        {
            Action result = () => VotingPair.Create("C#", "F#", "Haskell");
            Assert.ThrowsAny<InvalidOperationException>(result);
        }

        [Fact]
        public void Given_EmptyVotingPair_When_VoteForTopic_Then_Exception()
        {
            Action result = () => VotingPair.Empty().VoteForTopic("C#");
            Assert.ThrowsAny<InvalidOperationException>(result);
        }

        [Fact]
        public void Given_EmptyVotingPair_When_GetWinners_Then_Empty()
        {
            var result = VotingPair.Empty().GetWinners();
            Assert.Equal(result.Count(), 0);
        }

        [Fact]
        public void Given_NullVotingPair_When_GetWinners_Then_Empty()
        {
            var result = VotingPair.Create(null, null).GetWinners();
            Assert.Equal(result.Count(), 0);
        }

        [Fact]
        public void Given_NullVotingPair_When_VoteForTopic_Then_Exception()
        {
            Action result = () => VotingPair.Create(null, null).VoteForTopic("C#");
            Assert.ThrowsAny<InvalidOperationException>(result);
        }

        [Fact]
        public void Given_CreatedVotingPairFromDictionary_When_MoreThanTwoTopics_Then_Empty()
        {
            var result = VotingPair.CreateFrom(new Dictionary<string, int>()
            {
                {"C#", 0},
                {"F#", 1},
                {"Haskell", 2}
            });

            Assert.Equal(result.IsEmpty, true);
        }

        [Fact]
        public void Given_CreatedVotingPairFromDictionary_When_OnlyTwoTopics_Then_NewVotingPair()
        {
            var result = VotingPair.CreateFrom(new Dictionary<string, int>()
            {
                {"C#", 1},
                {"F#", 2}
            });

            Assert.Equal(result.TopicA, ("C#", 1));
            Assert.Equal(result.TopicB, ("F#", 2));
        }

        [Fact]
        public void Given_CreatedVotingPairFromDictionary_When_NullDictionary_Then_Exception()
        {
            Dictionary<string, int> values = null;
            Action result = () => VotingPair.CreateFrom(values);

            Assert.ThrowsAny<ArgumentNullException>(result);
        }

        [Fact]
        public void Given_CreatedVotingPair_When_Empty_Then_IsEmpty()
        {
            var result = VotingPair.Empty().IsEmpty;
            Assert.Equal(result, true);
        }

        [Fact]
        public void Given_CreatedVotingPair_When_NotEmpty_Then_IsNotEmpty()
        {
            var result = VotingPair.Create("not", "empty").IsEmpty;
            Assert.Equal(result, false);
        }
    }
}