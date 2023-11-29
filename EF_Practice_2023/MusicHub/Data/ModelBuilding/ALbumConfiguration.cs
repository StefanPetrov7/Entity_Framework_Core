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
    public class ALbumConfiguration : IEntityTypeConfiguration<Album>
    {
        public void Configure(EntityTypeBuilder<Album> builder)
        {
            builder.HasKey(x => x.Id);

            builder
                .Property(x => x.Name)
                .HasMaxLength(40)
                .IsRequired(true);

            builder
                .HasOne(x => x.Producer)
                .WithMany(x => x.Albums)
                .HasForeignKey(x => x.ProducerId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
