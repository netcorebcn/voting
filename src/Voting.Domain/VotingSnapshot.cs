using System;
using System.Collections.Generic;
using System.Linq;
using EasyEventSourcing.Aggregate;
using Voting.Domain.Events;

namespace Voting.Domain
{
    public class VotingSnapshot
    {
        public IDictionary<string, int> Topics { get; }

        public string Winner { get; }

        public VotingSnapshot (VotingPair votingPair, string winner)
        {
            Winner = winner;
            Topics = new Dictionary<string, int> ();

            if (!votingPair.IsEmpty)
            {
                Topics.Add(votingPair.TopicA.topic, votingPair.TopicA.votes);
                Topics.Add(votingPair.TopicB.topic, votingPair.TopicB.votes);
            }
        }

        public override string ToString() =>
            $@"{string.Join(",",Topics.Select(x => $"{x.Key}:{x.Value}"))}
            -{nameof(Winner)}{Winner}";
    }
}
