using System;

namespace Voting.Domain.Events
{
    public class VotingFinishedEvent : IVotingEvent
    {
        public Guid VotingId { get; }        
        public string Winner {get;}

        public VotingFinishedEvent(Guid votingId, string winner)
        {
            this.VotingId = votingId;
            this.Winner = winner;
        }
    }
}