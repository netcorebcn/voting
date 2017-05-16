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

        public VotingController(IRepository repository)
        {
            _repository = repository;
        }

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
        public async Task Vote(Guid id, [FromBody]string topic)
        {
            var voting = await _repository.GetById<VotingAggregate>(id);
            voting.VoteTopic(topic);
            await _repository.Save(voting);
        }

        [HttpDelete("{id}")]
        public async Task Start(Guid id)
        {
            var voting = await _repository.GetById<VotingAggregate>(id);
            voting.StartNextVoting();
            await _repository.Save(voting);
        }
    }
}
