namespace L6Backend.Models
{
    public class ChatGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Chat Chat { get; set; }
        public List<User> Users { get; set; }
    }
}
