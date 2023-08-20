namespace ApiServices.Models.Leaderboard
{
    public class LeaderboardRowData
    {
        public LeaderboardRowData(string name, int wins, int losses, int eliminations)
        {
            Name = name;
            Wins = wins;
            Losses = losses;
            Eliminations = eliminations;
        }

        public string Name { get; }
        
        public int Wins { get; }
        
        public int Losses { get; }
        
        public int Eliminations { get; }
    }
}