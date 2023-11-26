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
    public class TownConfiguration : IEntityTypeConfiguration<Town>
    {
        public void Configure(EntityTypeBuilder<Town> builder)
        {
            builder.HasKey(x => x.TownId);

            builder
                .HasOne(x => x.Country)
                .WithMany(x => x.Towns)
                .HasForeignKey(f => f.CountryId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
