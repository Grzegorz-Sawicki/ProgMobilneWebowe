import axios from 'axios';

export default async function handler(req, res) {
  const { id } = req.query;

  try {
    if (req.method === 'GET') {
      // Fetch messages for the given group ID
      const response = await axios.get(`http://localhost:5000/api/chat/${id}`);
      res.status(200).json(response.data);
    } else if (req.method === 'POST') {
      const { userId, postDate, text } = req.body;

      // Post a new message to the backend
      const response = await axios.post(`http://localhost:5000/api/chat/${id}`, {
        userId,
        postDate,
        text,
      });
      res.status(201).json(response.data);
    } else {
      // Handle unsupported HTTP methods
      res.status(405).end(); // Method Not Allowed
    }
  } catch (error) {
    // Error handling
    console.error(error);
    res.status(500).json({ error: 'An error occurred' });
  }
}