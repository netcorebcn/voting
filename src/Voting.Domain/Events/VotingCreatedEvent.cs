using System;

namespace Voting.Domain.Events
{
    public class VotingCreatedEvent
    {
        public string[] Topics { get; }
        public Guid VotingId { get; internal set; }

        public VotingCreatedEvent(Guid votingId, string[] topics)
        {
            VotingId = votingId;
            Topics = topics;
        }
    }
}