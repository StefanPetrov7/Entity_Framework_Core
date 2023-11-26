using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P01_StudentSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_StudentSystem.Data.ModelBuilding
{
    public class ResourceConfiguration : IEntityTypeConfiguration<Resource>
    {
        public void Configure(EntityTypeBuilder<Resource> builder)
        {
            builder.HasKey(x => x.ResourceId);

            builder
                .Property(x => x.Name)
                .HasMaxLength(50)
                .IsUnicode(true);

            builder
                .Property(x => x.Url)
                .IsUnicode(false);

            builder
                .HasOne(x => x.Course)
                .WithMany(x => x.Resources)
                .HasForeignKey(x => x.CourseId);

        }
    }
}
