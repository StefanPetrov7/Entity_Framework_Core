using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P02_FootballBetting.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.ModelBuilding
{
    public class BetConfiguration : IEntityTypeConfiguration<Bet>
    {
        public void Configure(EntityTypeBuilder<Bet> builder)
        {
            builder.HasKey(x => x.BetId);

            builder
                .HasOne(x => x.Game)
                .WithMany(b => b.Bets)
                .HasForeignKey(f => f.GameId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(x=>x.User)
                .WithMany(b=>b.Bets)
                .HasForeignKey(f => f.UserId)   
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .Property(p => p.Prediction)
                .IsRequired(true);
        }
    }
}
