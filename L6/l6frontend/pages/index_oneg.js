import { useState, useEffect } from 'react';
import axios from 'axios';
import * as signalR from '@microsoft/signalr';

export default function Home() {
  const [username, setUsername] = useState('');
  const [user, setUser] = useState(null);
  const [groups, setGroups] = useState([]);
  const [selectedGroup, setSelectedGroup] = useState(null);
  const [messages, setMessages] = useState([]);
  const [newMessage, setNewMessage] = useState('');
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
    if (selectedGroup && connection) {
      connection.invoke("JoinGroup", selectedGroup.id.toString())
        .catch(err => console.error(err.toString()));
  
      const receiveMessageHandler = (username, message) => {
        setMessages(prevMessages => [...prevMessages, { username, text: message, postDate: new Date().toISOString() }]);
        console.log(`You got a message: ${message}`);
      };
  
      connection.off("ReceiveMessage"); // Usunięcie istniejącego listenera
      connection.on("ReceiveMessage", receiveMessageHandler);
  
      return () => {
        connection.invoke("LeaveGroup", selectedGroup.id.toString())
          .catch(err => console.error(err.toString()));
        connection.off("ReceiveMessage", receiveMessageHandler); // Usunięcie listenera przy odmontowaniu
      };
    }
  }, [selectedGroup, connection]);

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
  
        // Dodanie listenera CreatedGroup
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
    const response = await axios.get(`/api/chatgroup/${groupId}`);
    setSelectedGroup(response.data);
    setMessages(response.data.chat.messages);
  };

  const handleSendMessage = async () => {

    connection.invoke("SendMessage", selectedGroup.id.toString(), user.name, newMessage)
      .catch(err => console.error(err.toString()));

    await axios.post(`/api/chat/${selectedGroup.id}`, {
      userId: user.id,
      postDate: new Date().toISOString(),
      text: newMessage,
    });
    setNewMessage('');
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
      {selectedGroup && (
        <div>
          <h2>Group: {selectedGroup.name}</h2>
          <div>
            {messages.map(message => (
              <div key={message.id}>
                <strong>{message.username}</strong>: {message.text} <em>{new Date(message.postDate).toLocaleString()}</em>
              </div>
            ))}
          </div>
          <input
            type="text"
            placeholder="Enter your message"
            value={newMessage}
            onChange={(e) => setNewMessage(e.target.value)}
          />
          <button onClick={handleSendMessage}>Send</button>
        </div>
      )}
    </div>
  );
}