namespace Voting.Domain.Events
{
    public class TopicVotedEvent : IDomainEvent
    {
        public string Topic { get; }

        public TopicVotedEvent(string topic)
        {
            this.Topic = topic;
        }
    }
}