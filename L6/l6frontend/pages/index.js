import { useState, useEffect } from 'react';
import axios from 'axios';
import * as signalR from '@microsoft/signalr';

export default function Home() {
  const [username, setUsername] = useState('');
  const [user, setUser] = useState(null);
  const [groups, setGroups] = useState([]);
  const [joinedGroups, setJoinedGroups] = useState([]);
  const [messages, setMessages] = useState({});
  const [groupMessages, setGroupMessages] = useState({});
  const [newGroup, setNewGroup] = useState('');
  const [connection, setConnection] = useState(null);

  useEffect(() => {
    if (user) {
      axios.get('/api/chatgroup').then(response => {
        setGroups(response.data);
      });
    }
  }, [user]);

  useEffect(() => {
    if (connection) {
      const receiveMessageHandler = (groupName, username, message) => {
        setMessages(prevMessages => ({
          ...prevMessages,
          [groupName]: [...(prevMessages[groupName] || []), { username, text: message, postDate: new Date().toISOString() }]
        }));
        console.log(`You got a message in ${groupName}: ${message}`);
      };

      connection.off("ReceiveMessage");
      connection.on("ReceiveMessage", receiveMessageHandler);

      return () => {
        connection.off("ReceiveMessage", receiveMessageHandler);
      };
    }
  }, [connection]);

  const handleLogin = async () => {
    const response = await axios.post('/api/user', { name: username });
    setUser(response.data);

    const newConnection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:5000/chathub')
      .withAutomaticReconnect()
      .build();

    newConnection.start()
      .then(() => {
        setConnection(newConnection);

        // Wywołaj metodę AssignUserToGroups w hubie SignalR
        //newConnection.invoke('AssignUserToGroups', user.id);

        const createdGroupHandler = (name) => {
          console.log(`New group was created: ${name}`);
          axios.get('/api/chatgroup').then(response => {
            setGroups(response.data);
          });
        };

        newConnection.off("CreatedGroup");
        newConnection.on("CreatedGroup", createdGroupHandler);
      })
      .catch(err => console.error(err.toString()));
  };

  const handleJoinGroup = async (groupId) => {
    const group = await axios.get(`/api/chatgroup/${groupId}`).then(response => response.data);

    // Sprawdź, czy użytkownik już jest w grupie
    if (joinedGroups.some(g => g.id === groupId)) {
      alert("You are already in this group.");
      return;
    }

    setJoinedGroups(prevGroups => [...prevGroups, group]);
    setMessages(prevMessages => ({
      ...prevMessages,
      [group.name]: group.chat.messages
    }));

    console.log(`/api/chatgroup/${groupId}/users/${user.id}`);

    await axios.post(`/api/chatgroup/${groupId}/users/${user.id}`);

    connection.invoke("JoinGroup", group.name)
      .catch(err => console.error(err.toString()));
  };

  const handleLeaveGroup = async (groupId, groupName) => {
    setJoinedGroups(prevGroups => prevGroups.filter(group => group.id !== groupId));
    setMessages(prevMessages => {
      const newMessages = { ...prevMessages };
      delete newMessages[groupName];
      return newMessages;
    });

    await axios.delete(`/api/chatgroup/${groupId}/users/${user.id}`);

    connection.invoke("LeaveGroup", groupName)
      .catch(err => console.error(err.toString()));
};

  const handleSendMessage = async (groupName) => {
    const group = joinedGroups.find(g => g.name === groupName);
    const message = groupMessages[groupName] || '';

    connection.invoke("SendMessage", groupName, user.name, message)
      .catch(err => console.error(err.toString()));

    await axios.post(`/api/chat/${group.id}`, {
      userId: user.id,
      postDate: new Date().toISOString(),
      text: message,
    });

    setGroupMessages(prevGroupMessages => ({
      ...prevGroupMessages,
      [groupName]: ''
    }));
  };

  const handleGroupMessageChange = (groupName, value) => {
    setGroupMessages(prevGroupMessages => ({
      ...prevGroupMessages,
      [groupName]: value
    }));
  };

  const handleCreateGroup = async () => {
    await axios.post(`/api/chatgroup`, {
      name: newGroup
    });
    setNewGroup('');
    connection.invoke("CreateGroup", newGroup)
      .catch(err => console.error(err.toString()));
  };

  if (!user) {
    return (
      <div>
        <h1>Login</h1>
        <input
          type="text"
          placeholder="Enter your username"
          value={username}
          onChange={(e) => setUsername(e.target.value)}
        />
        <button onClick={handleLogin}>Login</button>
      </div>
    );
  }

  return (
    <div>
      <h1>Welcome, {user.name}</h1>
      <h2>Create a Group</h2>
      <input
        type="text"
        placeholder="Enter group name"
        value={newGroup}
        onChange={(e) => setNewGroup(e.target.value)}
      />
      <button onClick={handleCreateGroup}>Create</button>
      <h2>Join a Group</h2>
      <ul>
        {groups.map(group => (
          <li key={group.id}>
            <button onClick={() => handleJoinGroup(group.id)}>{group.name}</button>
          </li>
        ))}
      </ul>
      <h2>Joined Groups</h2>
      {joinedGroups.map(group => (
        <div key={group.id}>
          <h3>Group: {group.name}</h3>
          <button onClick={() => handleLeaveGroup(group.id, group.name)}>Leave Group</button>
          <div>
            {(messages[group.name] || []).map((message, index) => (
              <div key={index}>
                <strong>{message.username}</strong>: {message.text} <em>{new Date(message.postDate).toLocaleString()}</em>
              </div>
            ))}
          </div>
          <input
            type="text"
            placeholder="Enter your message"
            value={groupMessages[group.name] || ''}
            onChange={(e) => handleGroupMessageChange(group.name, e.target.value)}
          />
          <button onClick={() => handleSendMessage(group.name)}>Send</button>
        </div>
      ))}
    </div>
  );
}
