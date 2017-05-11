namespace Voting.Domain.Events
{
    public class VotingFinishedEvent : IDomainEvent
    {
        public string Winner {get;}

        public VotingFinishedEvent(string winner) =>
            this.Winner = winner;
    }
}