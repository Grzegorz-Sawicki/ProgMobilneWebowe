import axios from 'axios';

export default async function handler(req, res) {
  if (req.method === 'GET') {
    const response = await axios.get('http://localhost:5000/api/chatgroup');
    res.status(200).json(response.data);
  }  else if (req.method === 'POST') {
    const { name } = req.body;
    const response = await axios.post('http://localhost:5000/api/chatgroup', { name });
    res.status(200).json(response.data);
  } else {
    res.status(405).end(); // Method Not Allowed
  }

}