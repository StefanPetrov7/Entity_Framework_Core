using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P02_FootballBetting.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.ModelBuilding
{
    public class GameConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder.HasKey(x => x.GameId);

            builder
                .HasOne(x => x.HomeTeam)
                .WithMany(g => g.HomeGames)
                .HasForeignKey(f => f.HomeTeamId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(x => x.AwayTeam)
                .WithMany(g => g.AwayGames)
                .HasForeignKey(f => f.AwayTeamId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
