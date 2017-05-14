namespace Voting.Domain.Events
{
    public class VotingCreatedEvent : IDomainEvent
    {
        public string[] Topics { get; }

        public VotingCreatedEvent(string[] topics)
        {
            this.Topics = topics;
        }
    }
}