using System.Collections.Generic;

namespace Voting.Domain.Events
{
    public class VotingStartedEvent : IDomainEvent
    {
        public List<string> Topics { get; }
        public List<(string topic, int votes)> VotingPair { get; } 

        public VotingStartedEvent(List<string> topics, List<(string topic, int votes)> votingPair)
        {
            Topics = topics;
            VotingPair = votingPair;
        }
    }
}