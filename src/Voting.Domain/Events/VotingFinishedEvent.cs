namespace Voting.Domain.Events
{
    public class VotingFinishedEvent
    {
        public string Winner {get;}

        public VotingFinishedEvent(string winner) =>
            this.Winner = winner;
    }
}