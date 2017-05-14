using System;
using System.Linq;
using Xunit;

namespace Voting.Domain.Tests
{
    public class VotingPairTest
    {
        [Fact]
        public void Given_NewVotingPair_When_VoteForTopic_Then_NewVotingPair()
        {
            var result = VotingPair.Create("C#", "F#").VoteForTopic("C#");
            Assert.Equal(result, new VotingPair(("C#", 1), ("F#", 0)));
        }

        [Fact]
        public void Given_CreatedVotingPair_When_VoteForTopic_Then_NewVotingPair()
        {
            var result = VotingPair.Create("C#", "F#").VoteForTopic("C#");
            Assert.Equal(result, new VotingPair(("C#", 1), ("F#", 0)));
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
    }
}