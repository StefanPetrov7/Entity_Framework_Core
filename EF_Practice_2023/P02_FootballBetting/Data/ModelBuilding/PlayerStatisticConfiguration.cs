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
    public class PlayerStatisticConfiguration : IEntityTypeConfiguration<PlayerStatistic>
    {
        public void Configure(EntityTypeBuilder<PlayerStatistic> builder)
        {
            builder.HasKey(x => new { x.PlayerId, x.GameId });

            builder
                .HasOne(x => x.Player)
                .WithMany(x => x.PlayersStatistics)
                .HasForeignKey(f => f.PlayerId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(x => x.Game)
                .WithMany(x => x.PlayersStatistics)
                .HasForeignKey(f => f.GameId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
