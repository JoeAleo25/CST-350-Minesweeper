namespace CST350_Minesweeper.Models
{
    public class SavedGame
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime SaveTime { get; set; }
        public string GameData { get; set; }
    }
}
