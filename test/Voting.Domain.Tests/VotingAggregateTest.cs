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
        public void Given_Topics_When_CreateVoting_Then_VotingCreated()
        {
            // Arrange
            var sut = new VotingAggregate();
            var topics = new [] {"C#", "F#", "Java", "JS"};

            // Act
            sut.CreateVoting(topics);
            
            // Assert
            var result = sut.GetPendingEvents().OfType<VotingCreatedEvent>().First();
            Assert.NotNull(result);
            Assert.Equal(result.Topics, topics);
        }

        [Fact]
        public void Given_CreatedVoting_When_StartNextVoting_Then_VotingStarted()
        {
            // Arrange
            var sut = new VotingAggregate();
            sut.CreateVoting("C#", "F#", "VB.NET", "PowerShell");

            // Act
            sut.StartNextVoting();

            // Assert
            var result = sut.GetPendingEvents().OfType<VotingStartedEvent>().First();
            Assert.NotNull(result);
            Assert.Equal(result.VotingPair.TopicA, ("C#", 0));
            Assert.Equal(result.VotingPair.TopicB, ("F#", 0));
            Assert.Equal(result.RemainingTopics, new[] {"VB.NET", "PowerShell"});
        }

        [Fact]
        public void Given_StartedVoting_CreatedWithTwoTopics_When_VoteTopic_Then_VotingFinished()
        {
            // Arrange
            var sut = new VotingAggregate();
            sut.CreateVoting("C#", "F#");
            sut.StartNextVoting();

            // Act
            sut.VoteTopic("C#");
            sut.StartNextVoting();

            // Assert
            var result = sut.GetPendingEvents().OfType<VotingFinishedEvent>().First();
            Assert.NotNull(result);
            Assert.Equal(result.Winner, "C#");
        }

        [Fact]
        public void Given_StartedVoting_When_VoteManyTimes_Then_VotingStarted()
        {
            // Arrange
            var sut = new VotingAggregate();
            sut.CreateVoting("C#", "F#");
            sut.StartNextVoting();

            // Act
            sut.VoteTopic("C#");
            sut.VoteTopic("F#");
            sut.StartNextVoting();
            
            // Assert
            var result = sut.GetPendingEvents().OfType<VotingStartedEvent>().First();
            Assert.NotNull(result);
            Assert.Equal(result.VotingPair.TopicA, ("C#", 0));
            Assert.Equal(result.VotingPair.TopicB, ("F#", 0));
        }

        [Fact]
        public void Given_FinishedVoting_When_StartVoting_Then_Exception()
        {
            // Arrange
            var sut = new VotingAggregate();

            sut.CreateVoting("C#", "F#");
            sut.StartNextVoting();
            sut.VoteTopic("C#");
            sut.StartNextVoting();

            // Act
            Action result = () => sut.StartNextVoting();

            // Assert
            Assert.ThrowsAny<InvalidOperationException>(result);
        }

        [Fact]
        public void Given_CreatedVoting_WithOneTopic_When_StartVoting_Then_VotingFinished()
        {
            // Arrange
            var sut = new VotingAggregate();
            sut.CreateVoting("C#");

            // Act
            sut.StartNextVoting();

            // Assert
            var result = sut.GetPendingEvents().OfType<VotingFinishedEvent>().First();
            Assert.NotNull(result);
            Assert.Equal(result.Winner, "C#");
        }

        [Fact]
        public void Given_StartedVoting_When_VoteForNonExistingTopic_Then_Exception()
        {
            // Arrange
            var sut = new VotingAggregate();
            sut.CreateVoting("C#", "F#");
            sut.StartNextVoting();

            // Act
            Action result = () => sut.VoteTopic("Haskell");

            // Assert
            Assert.ThrowsAny<InvalidOperationException>(result);
        }
    }
}
