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
    public class PerformerConfiguration : IEntityTypeConfiguration<Performer>
    {
        public void Configure(EntityTypeBuilder<Performer> builder)
        {
           builder.HasKey(x => x.Id);

            builder
                .Property(x => x.FirstName)
                .HasMaxLength(20)
                .IsRequired(true);

            builder
            .Property(x => x.LastName)
            .HasMaxLength(20)
            .IsRequired(true);


        }
    }
}
