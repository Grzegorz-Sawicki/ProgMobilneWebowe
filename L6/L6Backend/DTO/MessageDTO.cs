using L6Backend.Models;

namespace L6Backend.DTO
{
    public class MessageDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Text { get; set; }
        public DateTime PostDate { get; set; }
    }
}
