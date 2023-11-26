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
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(x => x.CourseId);

            builder
                .Property(x => x.Name)
                .HasMaxLength(80)
                .IsUnicode(true)
                .IsRequired();

            builder
                .Property(x => x.Description)
                .IsUnicode(true)
                .IsRequired(false);

            

        }
    }

}
