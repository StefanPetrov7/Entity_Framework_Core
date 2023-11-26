using Microsoft.EntityFrameworkCore;
using System;
using P01_StudentSystem.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace P01_StudentSystem.Data.ModelBuilding
{
    public class HomeworkConfiguration : IEntityTypeConfiguration<Homework>
    {
        public void Configure(EntityTypeBuilder<Homework> builder)
        {
            builder.HasKey(x => x.HomeworkId);

            builder
                .Property(x => x.Content)
                .IsUnicode(false);

            builder
                .HasOne(x => x.Student)
                .WithMany(x => x.Homeworks)
                .HasForeignKey(x => x.StudentId);

            builder
                .HasOne(x => x.Course)
                .WithMany(x => x.Homeworks)
                .HasForeignKey(x => x.CourseId);
        }
    }
}
