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
        public void Given_NoTopics_When_CreateVoting_Then_Exception()
        {
            // Arrange
            var sut = new VotingAggregate();
            
            // Act
            Action result = () => sut.CreateVoting(null);

            // Assert
            Assert.ThrowsAny<ArgumentNullException>(result);
        }

        [Fact]
        public void Given_Topics_When_CreateVoting_Then_VotingCreatedEvent()
        {
            // Arrange
            var sut = new VotingAggregate();
            var topics = new [] {"C#", "F#", "Java", "JS"};

            // Act
            sut.CreateVoting(topics);
            
            // Assert
            var result = sut.Events.OfType<VotingCreatedEvent>().First();
            Assert.NotNull(result);
            Assert.Equal(result.Topics, topics);
        }

        [Fact]
        public void Given_Topics_When_StartNextVoting_Then_VotingStartedEvent()
        {
            // Arrange
            var sut = new VotingAggregate();
            var topics = new [] {"C#", "F#", "VB.NET", "PowerShell"};

            // Act
            sut.CreateVoting(topics);
            sut.StartNextVoting();

            // Assert
            var result = sut.Events.OfType<VotingStartedEvent>().First();
            Assert.NotNull(result);
            Assert.Equal(result.VotingPair, new[] {("C#",0),("F#",0)});
            Assert.Equal(result.Topics, new[] {"VB.NET", "PowerShell"});
        }

        [Fact]
        public void Given_Topics_When_VoteTopic_Then_VotingFinishedEvent()
        {
            // Arrange
            var sut = new VotingAggregate();
            var topics = new [] {"C#", "F#"};

            // Act
            sut.CreateVoting(topics);
            sut.StartNextVoting();
            sut.VoteTopic("C#");
            sut.StartNextVoting();

            // Assert
            var result = sut.Events.OfType<VotingFinishedEvent>().First();
            Assert.NotNull(result);
            Assert.Equal(result.Winner, "C#");
        }

        [Fact]
        public void Given_2Topics_When_Vote2Topics_Then_VotingStartedEvent()
        {
            // Arrange
            var sut = new VotingAggregate();

            // Act
            sut.CreateVoting(new [] {"C#", "F#"});
            sut.StartNextVoting();
            sut.VoteTopic("C#");
            sut.VoteTopic("F#");
            sut.StartNextVoting();
            
            // Assert
            var result = sut.Events.OfType<VotingStartedEvent>().First();
            Assert.NotNull(result);
            Assert.Equal(result.VotingPair, new[] {("C#",0),("F#",0)});
        }

        [Fact]
        public void Given_FinishedVoting_When_StartVoting_Then_Exception()
        {
            // Arrange
            var sut = new VotingAggregate();
            var topics = new [] {"C#", "F#"};

            sut.CreateVoting(topics);
            sut.StartNextVoting();
            sut.VoteTopic("C#");
            sut.StartNextVoting();

            // Act
            Action result = () => sut.StartNextVoting();

            // Assert
            Assert.ThrowsAny<Exception>(result);
        }
    }
}
