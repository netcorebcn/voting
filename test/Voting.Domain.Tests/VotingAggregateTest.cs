using System;
using System.Collections.Generic;
using System.Linq;
using Voting.Domain.Events;
using Xunit;

namespace Voting.Domain.Tests
{
    public class VotingAggregateTest
    {
        [Fact]
        public void CreateVotingTest()
        {
            var sut = new VotingAggregate();
            var topics = new [] {"C#", "F#", "VB.NET", "PowerShell"};
            sut.CreateVoting(topics);

            var result = sut.Events.First();

            Assert.NotNull(result);
            Assert.IsType(typeof(VotingCreatedEvent), result);            
            Assert.Equal(((VotingCreatedEvent)result).Topics.ToArray(), topics);
        }

        [Fact]
        public void StartNextVotingTest()
        {
            var sut = new VotingAggregate();

            sut.CreateVoting(new [] {"C#", "F#", "VB.NET", "PowerShell"});
            sut.StartNextVoting();

            var result = sut.Events.OfType<VotingStartedEvent>().First();

            Assert.NotNull(result);
            Assert.Equal(result.VotingPair, new[] {("C#",0),("F#",0)});
            Assert.Equal(result.Topics, new[] {"VB.NET", "PowerShell"});
        }

        [Fact]
        public void VoteTopicTest()
        {
            var sut = new VotingAggregate();

            sut.CreateVoting(new [] {"C#", "F#"});
            sut.StartNextVoting();
            sut.VoteTopic("C#");
            sut.StartNextVoting();

            var result = sut.Events.OfType<VotingFinishedEvent>().First();

            Assert.NotNull(result);
            Assert.Equal(result.Winner, "C#");
        }
    }
}
