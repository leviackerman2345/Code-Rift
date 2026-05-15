namespace CodeRift.Models
{
    public class Question 
    {
        public int Id { get; set; }
        public int Level { get; set; }
        public required string Type { get; set; }
        public required string Text { get; set; }
        public string[] Choices { get; set; } = System.Array.Empty<string>();
        public required string CorrectAnswer { get; set; }
        public string[] OrderItems { get; set; } = System.Array.Empty<string>();
        public required string Explanation { get; set; }
        public int Damage { get; set; }
        public int EnemyDamage { get; set; }
    }
}
