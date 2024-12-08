import axios from 'axios';

export default async function handler(req, res) {
  const { groupId, userId } = req.query;

  try {
    if (req.method === 'POST') {
      // Dodaj użytkownika do grupy
      const response = await axios.post(`http://localhost:5000/api/chatgroup/${groupId}/users/${userId}`);
      res.status(204).end(); // No Content
    } else if (req.method === 'DELETE') {
      // Usuń użytkownika z grupy
      const response = await axios.delete(`http://localhost:5000/api/chatgroup/${groupId}/users/${userId}`);
      res.status(204).end(); // No Content
    } else {
      // Obsługa nieobsługiwanych metod HTTP
      res.status(405).end(); // Method Not Allowed
    }
  } catch (error) {
    // Obsługa błędów
    console.error(error);
    res.status(500).json({ error: 'An error occurred' });
  }
}