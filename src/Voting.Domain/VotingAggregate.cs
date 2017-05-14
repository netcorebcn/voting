using System;
using System.Collections.Generic;
using System.Linq;
using Voting.Domain.Events;

namespace Voting.Domain
{
    public class VotingAggregate
    {
        public List<IDomainEvent> Events = new List<IDomainEvent>();
        
        private string[] _topics;

        private VotingPair _votingPair;

        private string _winner;

        public void CreateVoting (params string[] topics)
        {
            topics = topics ?? throw new ArgumentNullException(nameof(topics)); 
            Apply(new VotingCreatedEvent(topics));
        }

        public void StartNextVoting()
        {
            if (!string.IsNullOrEmpty(_winner))
                throw new InvalidOperationException($"The voting is over, the winner was {_winner}");

            _topics = _topics.Concat(_votingPair.GetWinners()).ToArray();

            if (_topics.Count() == 1)
                Apply(new VotingFinishedEvent(winner:_topics.Single()));
            else
                Apply(new VotingStartedEvent(
                       _topics.Skip(2).ToArray(), 
                        VotingPair.Create(_topics.Take(2).ToArray())));
        }

        public void VoteTopic (string topic) =>
            Apply(new TopicVotedEvent(topic));

        public void Apply(VotingCreatedEvent @event)
        {
            _topics = @event.Topics;
            _votingPair = VotingPair.Empty();
            _winner = string.Empty;
            Events.Add(@event);
        }

        public void Apply(VotingStartedEvent @event)
        {
            _votingPair = @event.VotingPair;
            _topics = @event.RemainingTopics;
            Events.Add(@event);
        }

        public void Apply(TopicVotedEvent @event)
        {
            _votingPair = _votingPair.VoteForTopic(@event.Topic);
            Events.Add(@event);
        }

        public void Apply(VotingFinishedEvent @event) 
        {
            _winner = @event.Winner;
            Events.Add(@event);
        }
    }
}
