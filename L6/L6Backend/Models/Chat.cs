using L6Backend.Models;

namespace L6Backend.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public List<Message> Messages { get; set; }
    }
}
