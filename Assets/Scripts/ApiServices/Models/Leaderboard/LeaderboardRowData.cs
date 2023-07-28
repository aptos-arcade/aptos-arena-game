namespace Leaderboard
{
    public class LeaderboardRowData
    {
        public LeaderboardRowData(string name, int wins, int losses)
        {
            Name = name;
            Wins = wins;
            Losses = losses;
        }

        public string Name { get; }
        
        public int Wins { get; }
        
        public int Losses { get; }
    }
}