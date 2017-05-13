using System;
using System.Collections.Generic;
using System.Linq;
using Voting.Domain.Events;

namespace Voting.Domain
{
    public class VotingAggregate
    {
        public List<IDomainEvent> Events = new List<IDomainEvent>();
        
        private List<string> _topics;
        private List<(string topic, int votes)> _votingPair;
        private string _winner;

        public void CreateVoting (string[] topics)
        {
            topics = topics ?? throw new ArgumentNullException(nameof(topics)); 
            Apply(new VotingCreatedEvent(topics.ToList()));
        }

        public void StartNextVoting()
        {
            if (!string.IsNullOrEmpty(_winner))
                throw new Exception($"The voting is over, the winner was {_winner}");

            _topics = _topics.Concat(GetWinners()).ToList();

            if (_topics.Count == 1)
            {
                Apply(new VotingFinishedEvent(winner:_topics.Single()));
            }
            else
            {
                Apply(new VotingStartedEvent(
                    _topics.Skip(2).ToList(), 
                    _topics.Take(2).Select(t => (t, 0)).ToList()));
            }

            IEnumerable<string> GetWinners()
            {
                if (!_votingPair.Any()) return new List<string>();
                
                var maxVotes = _votingPair.Max(t => t.votes);
                return _votingPair.Where(x => x.votes == maxVotes).Select(x => x.topic);        
            }
        }

        public void VoteTopic (string topic)
        {
            if (!_votingPair.Any(x => x.topic == topic))
                throw new Exception($"Selected topic {topic} is not a valid option for voting");
            
            Apply(new TopicVotedEvent(topic));
        }

        public void Apply(VotingCreatedEvent @event)
        {
            _topics = @event.Topics;
            _votingPair = new List<(string, int)>();
            _winner = string.Empty;
            Events.Add(@event);
        }

        public void Apply(VotingStartedEvent @event)
        {
            _votingPair = @event.VotingPair;
            _topics = @event.Topics;
            Events.Add(@event);
        }

        public void Apply(TopicVotedEvent @event)
        {
            _votingPair = new List<(string, int)> {
                _votingPair.Where(x => x.topic == @event.Topic)
                    .Select(x => (x.topic, ++x.votes)).First(), 
                _votingPair.Last()};
            Events.Add(@event);
        }

        public void Apply(VotingFinishedEvent @event) 
        {
            _winner = @event.Winner;
            Events.Add(@event);
        }
    }
}
