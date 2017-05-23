using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyEventSourcing.Aggregate;
using Voting.Domain.Events;
using Xunit;

namespace Voting.Domain.Tests
{
    public class VotingReadModelServiceTest
    {
        [Fact]
        public async Task Given_CreatedVoting_When_StartAndVoteAndFinish_Then_SameSnapshots()
        {
            var sut = new VotingReadModelService(new FakeRepo());
            await sut.AddOrUpdate(GetCreatedEvent());

            var snapshot = await sut.AddOrUpdate(new VotingStartedEvent(votingId, new string[] { }, VotingPair.Create("C#", "F#")));
            var result = await sut.Get(votingId);
            Assert.Equal(result.Topics, snapshot.Topics);

            snapshot = await sut.AddOrUpdate(new TopicVotedEvent(votingId, "C#"));
            result = await sut.Get(votingId);
            Assert.Equal(result.Topics, snapshot.Topics);

            snapshot = await sut.AddOrUpdate(new VotingFinishedEvent(votingId, "C#"));
            result = await sut.Get(votingId);
            Assert.Equal(result.Topics, snapshot.Topics);
            Assert.Equal(result.Winner, snapshot.Winner);
        }

        private static readonly Guid votingId = Guid.NewGuid();

        public static VotingCreatedEvent GetCreatedEvent() => new VotingCreatedEvent(votingId, new string[] { "C#", "F#" });

        public class FakeRepo : IRepository
        {
            public Task<TAggregate> GetById<TAggregate>(Guid id) where TAggregate : IAggregate, new()
            {
                var aggregate = new TAggregate();
                aggregate.ApplyEvent(GetCreatedEvent());
                return Task.FromResult(aggregate);
            }

            public Task<int> Save(IAggregate aggregate) =>
                Task.FromResult(0);
        }
    }
}
