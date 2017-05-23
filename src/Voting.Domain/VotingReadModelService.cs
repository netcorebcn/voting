using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using EasyEventSourcing.Aggregate;
using Voting.Domain;
using Voting.Domain.Events;

namespace Voting.Domain
{
    public class VotingReadModelService
    {
        private readonly ConcurrentDictionary<Guid, VotingSnapshot> _readModels;
        private readonly IRepository _repository;

        public VotingReadModelService(IRepository repository)
        {
            _repository = repository;
            _readModels = new ConcurrentDictionary<Guid, VotingSnapshot>();
        }

        public async Task<VotingSnapshot> AddOrUpdate(object @event)
        {
            var aggregate = await GetVotingAggregate(@event);
            _readModels[aggregate.Id] = aggregate.CreateSnapshot();
            return _readModels[aggregate.Id];
        }

        public async Task<VotingSnapshot> Get(Guid votingId) =>
            _readModels.TryGetValue(votingId, out VotingSnapshot snapshot)
                ? snapshot
                : await GetFromRepository(votingId);

        private async Task<VotingSnapshot> GetFromRepository(Guid votingId)
        {
            var voting = await _repository.GetById<VotingAggregate>(votingId);
            return voting.CreateSnapshot();
        }

        private async Task<VotingAggregate> GetVotingAggregate(object @event)
        {
            var votingId = ((IVotingEvent)@event).VotingId;

            if (!_readModels.TryGetValue(votingId, out VotingSnapshot current)) 
                return await _repository.GetById<VotingAggregate>(votingId);

            var aggregate = VotingAggregate.CreateFrom(current);
            aggregate.Apply((dynamic)@event);
            return aggregate;
        }
    }
}