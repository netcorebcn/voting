const url = `//${window.location.hostname}:81/api/voting/`;
const ws = `ws://${window.location.hostname}:81/ws`;

export const getVoting = (votingId) =>
    fetch(`${url}${votingId}`).then(response => {
        if (response.status === 204) {
            return null;
        }
        if (response.status === 200) {
            return response.json();
        }
    });

export const createVoting = (topics) =>
    fetch(url, {
        method: 'PUT',
        headers: {
            Accept: 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(topics)
    }).then(r => r.json());

export const startVoting = (votingId) =>
    fetch(`${url}${votingId}`, {
        method: 'DELETE',
        headers: {
            Accept: 'application/json',
            'Content-Type': 'application/json'
        }
    }).then(r => r.json());

export const vote = (votingId, topic) =>
    fetch(`${url}${votingId}`, {
        method: 'POST',
        headers: {
            Accept: 'application/json',
            'Content-Type': 'application/json'
        },
        body: `'${topic}'`
    }).then(r => r.json());

export const subscribeToVotingEvents = cb => {
    const webSocket = new WebSocket(ws);
    webSocket.onopen = e => {
        console.log(e);
    };
    webSocket.onmessage = e => {
        console.log(e);
        if (e.data.indexOf('Connected') === -1) cb(JSON.parse(e.data));
    };
};
