using System.Collections.Generic;

namespace Voting.Domain.Events
{
    public class VotingCreatedEvent : IDomainEvent
    {
        public List<string> Topics { get; }

        public VotingCreatedEvent(List<string> topics)
        {
            this.Topics = topics;
        }
    }
}