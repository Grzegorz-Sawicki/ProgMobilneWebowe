namespace L6Backend.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ChatGroup> ChatGroups { get; set; }
        public List<Message> Messages { get; set; }
    }
}
