using System;

namespace Voting.Domain.Events
{
    public class TopicVotedEvent : IVotingEvent
    {
        public Guid VotingId { get; }
        public string Topic { get; }

        public TopicVotedEvent(Guid votingId, string topic)
        {
            VotingId = votingId;
            this.Topic = topic;
        }
    }
}