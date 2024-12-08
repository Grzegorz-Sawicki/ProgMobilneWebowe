namespace L6Backend.DTO
{
    public class CreateMessageDTO
    {
        public int UserId { get; set; }
        public DateTime PostDate { get; set; }
        public string Text { get; set; }
    }
}
