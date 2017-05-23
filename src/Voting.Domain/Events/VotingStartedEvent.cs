using System;

namespace Voting.Domain.Events
{
    public class VotingStartedEvent
    {
        public Guid VotingId { get; }
        public string[] RemainingTopics { get; }
        public VotingPair VotingPair { get; } 

        public VotingStartedEvent(Guid votingId, string[] remainingTopics, VotingPair votingPair)
        {
            VotingId = votingId;
            RemainingTopics = remainingTopics;
            VotingPair = votingPair;
        }
    }
}