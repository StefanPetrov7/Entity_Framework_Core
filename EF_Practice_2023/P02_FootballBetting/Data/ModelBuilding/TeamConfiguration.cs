using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P02_FootballBetting.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.ModelBuilding
{
    public class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.HasKey(x => x.TeamId);

            builder
                .HasOne(x => x.PrimaryKitColor)
                .WithMany(c => c.PrimaryKitTeams)
                .HasForeignKey(f => f.PrimaryKitColorId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(x => x.SecondaryKitColor)
                .WithMany(c => c.SecondaryKitTeams)
                .HasForeignKey(f => f.SecondaryKitColorId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(x => x.Town)
                .WithMany(t => t.Teams)
                .HasForeignKey(f => f.TownId)
                .OnDelete(DeleteBehavior.NoAction); 
        }
    }
}
