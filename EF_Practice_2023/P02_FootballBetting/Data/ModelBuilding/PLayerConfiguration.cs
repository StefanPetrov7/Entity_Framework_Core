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
    public class PLayerConfiguration : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            builder.HasKey(x => x.PlayerId);

            builder
                .HasOne(x => x.Team)
                .WithMany(p => p.Players)
                .HasForeignKey(f => f.TeamId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(x => x.Position)
                .WithMany(p => p.Players)
                .HasForeignKey(f => f.PositionId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(x => x.Town)
                .WithMany(p => p.Players)
                .HasForeignKey(f => f.TownId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
