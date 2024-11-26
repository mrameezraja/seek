namespace Seek.Entities
{
    public class Setting
    {
        public Guid Id { get; set; }
        public string Course { get; set; }
        public string? Chapter { get; set; }
        public string? Lesson { get; set; }
        // public int? Time { get; set; } = 0;
    }
}
