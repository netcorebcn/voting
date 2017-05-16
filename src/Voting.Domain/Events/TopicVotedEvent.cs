namespace Voting.Domain.Events
{
    public class TopicVotedEvent
    {
        public string Topic { get; }

        public TopicVotedEvent(string topic)
        {
            this.Topic = topic;
        }
    }
}