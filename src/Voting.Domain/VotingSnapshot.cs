using System;
using System.Collections.Generic;
using System.Linq;
using EasyEventSourcing.Aggregate;
using Voting.Domain.Events;

namespace Voting.Domain
{
    public class VotingSnapshot
    {
        public Guid VotingId { get; }

        public IDictionary<string, int> Topics { get; }

        public string Winner { get; }

        public VotingSnapshot (Guid votingId, VotingPair votingPair, string winner)
        {
            VotingId = votingId;
            Winner = winner;

            votingPair = votingPair ?? VotingPair.Empty();
            Topics = new Dictionary<string, int> ();
            if (!votingPair.IsEmpty)
            {
                Topics.Add(votingPair.TopicA.topic, votingPair.TopicA.votes);
                Topics.Add(votingPair.TopicB.topic, votingPair.TopicB.votes);
            }
        }

        public override string ToString() => 
            $@"{nameof(Topics)}:{string.Join(",",Topics.Select(x => $"{x.Key}:{x.Value}"))}
            {nameof(Winner)}{Winner}";
    }
}
