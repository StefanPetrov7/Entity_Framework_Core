namespace MusicHub.Data
{
    using Microsoft.EntityFrameworkCore;
    using MusicHub.Data.ModelBuilding;
    using MusicHub.Data.Models;

    public class MusicHubDbContext : DbContext
    {
        public MusicHubDbContext()
        { }

        public MusicHubDbContext(DbContextOptions options) : base(options)
        { }

        public DbSet<Album> Albums { get; set; }

        public DbSet<Performer> Performers { get; set; }

        public DbSet<Producer> Producers { get; set; }

        public DbSet<Song> Songs { get; set; }

        public DbSet<SongPerformer> SongsPerformers { get; set; }

        public DbSet<Writer> Writers { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured == false)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ALbumConfiguration());

            modelBuilder.ApplyConfiguration(new PerformerConfiguration());

            modelBuilder.ApplyConfiguration(new ProducerConfiguration());

            modelBuilder.ApplyConfiguration(new SongConfiguration());

            modelBuilder.ApplyConfiguration(new SongPerformerConfiguration());

            modelBuilder.ApplyConfiguration(new WriterConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
