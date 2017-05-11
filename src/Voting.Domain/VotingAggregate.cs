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

        public void CreateVoting (string[] topics) => 
            Apply(new VotingCreatedEvent(topics.ToList()));

        public void StartNextVoting()
        {
            _topics = _topics.Concat(GetWinners(_votingPair)).ToList();

            if (_topics.Count == 1) 
                Apply(new VotingFinishedEvent(winner:_topics.Single()));
            else 
                Apply(new VotingStartedEvent(
                    _topics.Skip(2).ToList(), 
                    _topics.Take(2).Select(t => (t, 0)).ToList()));

            IEnumerable<string> GetWinners(List<(string topic, int votes)> votingTopics)
            {
                if (votingTopics == null) return new List<string>();
                var maxVotes = votingTopics.Max(t => t.votes);
                return votingTopics.Where(x => x.votes == maxVotes).Select(x => x.topic);        
            }
        }

        public void VoteTopic (string topic) =>
            Apply(new TopicVotedEvent(topic));

        public void Apply(VotingCreatedEvent @event)
        {
            _topics = @event.Topics;
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
                _votingPair.Where(x => x.topic == @event.Topic).Select(x => (x.topic, ++x.votes)).First(), 
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
