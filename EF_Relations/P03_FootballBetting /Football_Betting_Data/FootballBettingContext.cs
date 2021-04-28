using Microsoft.EntityFrameworkCore;


namespace P03_FootballBetting.Data
{
    public class FootballBettingContext : DbContext
    {
        public FootballBettingContext()
        { }

        public FootballBettingContext(DbContextOptions option) : base(option)
        { }

        public DbSet<User> Users { get; set; }

        public DbSet<Bet> Bets { get; set; }

        public DbSet<Game> Games { get; set; }

        public DbSet<PlayerStatistic> PlayerStatistics { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Color> Colors { get; set; }

        public DbSet<Position> Positions { get; set; }

        public DbSet<Town> Towns { get; set; }

        public DbSet<Country> Countries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=localhost;Database=BookmakerDB;user=sa;Password=Password123@jkl#");
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bet>()           // one user can have many bets 
                .HasOne(x => x.User)
                .WithMany(x => x.Bets)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Bet>()          // one game can have many bets
                .HasOne(x => x.Game)
                .WithMany(x => x.Bets)
                .HasForeignKey(x => x.GameId);

            modelBuilder.Entity<Player>()       // player one to many with possitions
                .HasOne(x => x.Position)
                .WithMany(x => x.Players)
                .HasForeignKey(x => x.PossitionId)
                  .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Town>()      // town one to many with country
                .HasOne(x => x.Country)
                .WithMany(x => x.Towns)
                .HasForeignKey(x => x.CountryId)
                  .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Team>()        // primaryColor to color
                .HasOne(x => x.PrimaryKitColor)
                .WithMany(x => x.PrimaryKitTeams)
                .HasForeignKey(x => x.PrimaryKitColorId)
                  .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Team>()        // secondary to color
              .HasOne(x => x.SecondaryKitColor)
              .WithMany(x => x.SecondaryKitTeams)
              .HasForeignKey(x => x.SecondaryKitColorId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Team>()     // team to town => one to many 
                .HasOne(x => x.Town)
                .WithMany(x => x.Teams)
                .HasForeignKey(x => x.TownId)
                  .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Player>()    // player => team one to many 
                .HasOne(x => x.Team)
                .WithMany(x => x.Players)
                .HasForeignKey(x => x.TeamId)
                  .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Game>()   // game => home team one to many 
                .HasOne(x => x.HomeTeam)
                .WithMany(x => x.HomeGames)
                .HasForeignKey(x => x.HomeTeamId)
                  .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Game>()   // game => away team one to many 
             .HasOne(x => x.AwayTeam)
             .WithMany(x => x.AwayGames)
             .HasForeignKey(x => x.AwayTeamId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PlayerStatistic>()
                .HasKey(x => new { x.PlayerId, x.GameId });

            base.OnModelCreating(modelBuilder);
        }
    }
}
