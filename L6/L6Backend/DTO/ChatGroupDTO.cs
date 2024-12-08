using L6Backend.Models;

namespace L6Backend.DTO
{
    public class ChatGroupDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ChatDTO Chat { get; set; }
    }
}
