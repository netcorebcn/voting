using System;

namespace Voting.Domain.Events
{
    public class VotingCreatedEvent
    {
        public Guid VotingId { get; }
        public string[] Topics { get; }

        public VotingCreatedEvent(Guid votingId, string[] topics)
        {
            VotingId = votingId;
            Topics = topics;
        }
    }
}