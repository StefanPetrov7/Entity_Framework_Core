using P02_FootballBetting.Data;

namespace P02_FootballBetting
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            FootballBettingContext fbDB= new FootballBettingContext();
            fbDB.Database.EnsureDeleted();
            fbDB.Database.EnsureCreated();
        }
    }
}
