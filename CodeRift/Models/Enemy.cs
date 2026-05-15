namespace CodeRift.Models
{
    public class Enemy 
    {
        public required string Name { get; set; }
        public int MaxHP { get; set; }
        public int CurrentHP { get; set; }
        public string SpritePath { get; set; } = string.Empty;
        public required string Description { get; set; }
    }
}
