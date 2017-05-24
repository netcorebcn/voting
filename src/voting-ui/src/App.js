import React, { Component } from 'react';
import './App.css';
import { getVoting, createVoting, startVoting, vote, subscribeToVotingEvents } from './api';

class App extends Component {
  constructor(props) {
    super(props);
    this.state = {
      votings: [],
      topics: '',
      votingModel: { topics: {} }
    };

    this.createVotingHandler = this.createVotingHandler.bind(this);
    this.startVotingHandler = this.startVotingHandler.bind(this);
    this.topicsChangeHandler = this.topicsChangeHandler.bind(this);
    this.voteHandler = this.voteHandler.bind(this);
  }

  componentDidMount() {
    subscribeToVotingEvents(votingModel => {
      this.setState({
        ...this.state,
        votingModel
      });
    });
  }

  createVotingHandler() {
    createVoting(this.state.topics.split(','))
      .then(votingId =>
        this.setState(
          {
            ...this.state,
            votings: [...this.state.votings, votingId]
          }));
  }

  startVotingHandler(votingId) {
    startVoting(votingId).then();
  }

  voteHandler(topic) {
    const votingId = this.state.votingModel.votingId;
    vote(votingId, topic).then(
      // getVoting(votingId).then(
      //   votingModel =>
      //     this.setState(
      //       {
      //         ...this.state,
      //         currentVoting: votingModel
      //       }))
    );
  }

  topicsChangeHandler(event) {
    this.setState({ topics: event.target.value });
  }

  render() {
    console.log(this.state);
    return (
      <div className="App">
        <div className="App-header">
          <h2>Welcome to Voting App</h2>
        </div>
        <p className="App-intro">
          <input type="text" name="name" value={this.state.topics} onChange={this.topicsChangeHandler} />
          <button onClick={this.createVotingHandler}>
            Submit
          </button>

          <ul>
            {this.state.votings.map(id => <Voting key={id} votingId={id} startVoting={this.startVotingHandler} />)}
          </ul>

          <ul>
            {this.state.topics.split(',').map(topic =>
              <Topic key={topic} topic={topic}
                votes={this.state.votingModel.topics[topic]}
                voteHandler={this.voteHandler} />)} 
          </ul>
          <div>Winner is :{this.state.votingModel.winner}</div>
        </p>
      </div>
    );
  }
}

const Voting = ({ votingId, startVoting }) =>
  <li onClick={() => startVoting(votingId)}>{votingId}</li>

const Topic = ({ topic, votes, voteHandler }) =>
  <li onClick={() => voteHandler(topic)}>{topic} Votes: {votes}</li>

export default App;
