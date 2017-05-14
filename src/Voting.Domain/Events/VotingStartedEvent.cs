namespace Voting.Domain.Events
{
    public class VotingStartedEvent : IDomainEvent
    {
        public string[] RemainingTopics { get; }
        public VotingPair VotingPair { get; } 

        public VotingStartedEvent(string[] remainingTopics, VotingPair votingPair)
        {
            RemainingTopics = remainingTopics;
            VotingPair = votingPair;
        }
    }
}