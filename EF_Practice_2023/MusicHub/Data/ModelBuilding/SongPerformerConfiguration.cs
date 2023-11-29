using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicHub.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicHub.Data.ModelBuilding
{
    public class SongPerformerConfiguration : IEntityTypeConfiguration<SongPerformer>
    {
        public void Configure(EntityTypeBuilder<SongPerformer> builder)
        {
            builder.HasKey(x => new { x.SongId, x.PerformerId });

            builder
                .HasOne(x => x.Performer)
                .WithMany(x => x.PerformerSongs)
                .HasForeignKey(x => x.PerformerId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(x => x.Song)
                .WithMany(x => x.SongPerformers)
                .HasForeignKey(x => x.SongId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
