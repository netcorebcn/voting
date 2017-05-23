using System;

namespace Voting.Domain.Events
{
    public interface IVotingEvent
    {
        Guid VotingId { get; }
    }
}