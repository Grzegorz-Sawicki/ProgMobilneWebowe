import axios from 'axios';

export default async function handler(req, res) {
  const { groupId } = req.query;
  if (req.method === 'GET') {
    const response = await axios.get(`http://localhost:5000/api/chatgroup/${groupId}`);
    res.status(200).json(response.data);
  } else {
    res.status(405).end(); // Method Not Allowed
  }
}