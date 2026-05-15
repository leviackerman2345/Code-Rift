namespace CodeRift.Models
{
    public class Player 
    {
        public required string Name { get; set; }
        public int MaxHP { get; set; }
        public int CurrentHP { get; set; }
        public int Level { get; set; }
        public int Score { get; set; }
        public int CorrectAnswers { get; set; }
        public int WrongAnswers { get; set; }
    }
}
