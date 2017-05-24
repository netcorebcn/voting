using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyEventSourcing.Aggregate;
using Microsoft.AspNetCore.Mvc;
using Voting.Domain;

namespace Voting.Api.Controllers
{
    [Route("api/[controller]")]
    public class VotingController
    {
        private readonly IRepository _repository;
        private readonly VotingReadModelService _readModelService;

        public VotingController(IRepository repository, VotingReadModelService readModelService)
        {
            _repository = repository;
            _readModelService = readModelService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<VotingSnapshot> GetVoting(Guid id)  => await _readModelService.Get(id);

        [HttpPut]
        public async Task<Guid> Create([FromBody] string[] topics)
        {
            var voting = new VotingAggregate();
            voting.CreateVoting(topics);
            await _repository.Save(voting);
            return voting.Id;
        }

        [HttpPost]
        [Route("{id}")]
        public async Task<Guid> Vote(Guid id, [FromBody]string topic)
        {
            var voting = await _repository.GetById<VotingAggregate>(id);
            voting.VoteTopic(topic);
            await _repository.Save(voting);
            return voting.Id;
        }

        [HttpDelete("{id}")]
        public async Task<Guid> Start(Guid id)
        {
            var voting = await _repository.GetById<VotingAggregate>(id);
            voting.StartNextVoting();
            await _repository.Save(voting);
            return voting.Id;
        }
    }
}
