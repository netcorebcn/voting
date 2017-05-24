using System;
using System.Collections.Generic;
using System.Linq;
using EasyEventSourcing.Aggregate;
using Voting.Domain.Events;

namespace Voting.Domain
{
    public class VotingAggregate : AggregateRoot
    {
        private string[] _topics;

        private VotingPair _votingPair;

        private string _winner;

        public static VotingAggregate CreateFrom(VotingSnapshot votingSnapshot)
        {
            var votingAggregate = new VotingAggregate();
            votingAggregate._winner = votingSnapshot.Winner;
            votingAggregate._votingPair = VotingPair.CreateFrom(votingSnapshot.Topics);
            return votingAggregate;
        }

        public VotingSnapshot CreateSnapshot() => new VotingSnapshot(Id,_votingPair, _winner);

        public void CreateVoting(params string[] topics)
        {
            topics = topics ?? throw new ArgumentNullException(nameof(topics));
            if (topics.Distinct().Count() != topics.Count()) throw new InvalidOperationException("Duplicated topics are not allowed");

            RaiseEvent(new VotingCreatedEvent(Id, topics));
        }

        public void StartNextVoting()
        {
            AssertNotFinishedVoting();
            _topics = _topics.Concat(_votingPair.GetWinners()).ToArray();

            if (_topics.Count() == 1)
                RaiseEvent(new VotingFinishedEvent(Id, _topics.Single()));
            else
                RaiseEvent(new VotingStartedEvent(Id, _topics.Skip(2).ToArray(), VotingPair.Create(_topics.Take(2).ToArray())));
        }

        public void VoteTopic(string topic)
        {
            AssertNotFinishedVoting();
            RaiseEvent(new TopicVotedEvent(Id, topic));
        }

        private void AssertNotFinishedVoting()
        {
            if (!string.IsNullOrEmpty(_winner)) throw new InvalidOperationException($"The voting is over, the winner was {_winner}");
        }

        public void Apply(VotingCreatedEvent @event)
        {
            Id = @event.VotingId;
            _topics = @event.Topics;
            _votingPair = VotingPair.Empty();
            _winner = string.Empty;
        }

        public void Apply(VotingStartedEvent @event)
        {
            Id = @event.VotingId;
            _votingPair = @event.VotingPair;
            _topics = @event.RemainingTopics;
        }

        public void Apply(TopicVotedEvent @event)
        {
            Id = @event.VotingId;
            _votingPair = _votingPair.VoteForTopic(@event.Topic);
        }

        public void Apply(VotingFinishedEvent @event)
        {
            Id = @event.VotingId;
            _winner = @event.Winner;
        }
    }
}
