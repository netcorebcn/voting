using System;
using System.Collections.Generic;
using System.Linq;

namespace Voting.Domain
{
    public class VotingPair
    {
        public (string topic, int votes) TopicA { get; }
        public (string topic, int votes) TopicB { get; }

        public VotingPair((string, int) topicA, (string, int) topicB)
        {
            TopicA = topicA;
            TopicB = topicB;
        }

        public static VotingPair Empty() => 
            VotingPair.Create(string.Empty, string.Empty);

        public bool IsEmpty => string.IsNullOrEmpty(TopicA.topic) || string.IsNullOrEmpty(TopicB.topic);

        public static VotingPair CreateFrom(IDictionary<string, int> topics)
        {
            topics = topics ?? throw new ArgumentNullException(nameof(topics));
            if (topics.Count() != 2) return VotingPair.Empty();
           
            var topicA = topics.First();
            var topicB = topics.Last();
            return new VotingPair((topicA.Key, topicA.Value), (topicB.Key, topicB.Value));
        }

        public static VotingPair Create(params string[] topics)
        {
            topics = topics ?? throw new ArgumentNullException(nameof(topics));
            if (topics.Count() != 2)
                throw new InvalidOperationException("Incorrect number of topics");

            var map = topics.Select(t => (t, 0));
            return new VotingPair(map.First(), map.Last());
        }

        public IEnumerable<string> GetWinners()
        {
            if (string.IsNullOrEmpty(TopicA.topic) || string.IsNullOrEmpty(TopicB.topic))
                return new string[] { };

            var map = new List<(string topic, int votes)> { TopicA, TopicB };
            var maxVotes = map.Max(t => t.votes);
            return map.Where(x => x.votes == maxVotes).Select(x => x.topic);        
        }

        public VotingPair VoteForTopic(string topic) =>
            !IsValid(topic)
            ? throw new InvalidOperationException($"Selected topic {topic} is not a valid option for voting")              
            : TopicA.topic == topic
                ? new VotingPair((TopicA.topic, TopicA.votes + 1), TopicB)
                : new VotingPair(TopicA, (TopicB.topic, TopicB.votes + 1));    

        private bool IsValid(string topic) =>
            !string.IsNullOrEmpty(topic) && 
            (TopicA.topic == topic || TopicB.topic == topic);
    }
}