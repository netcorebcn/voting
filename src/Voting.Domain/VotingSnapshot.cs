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
            Topics = new Dictionary<string, int> {
                { votingPair.TopicA.topic, votingPair.TopicA.votes },
                { votingPair.TopicB.topic, votingPair.TopicB.votes },
            };
        }
    }
}
