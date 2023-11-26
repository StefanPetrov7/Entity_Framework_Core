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
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder
                 .HasKey(x => x.StudentId);

            builder
                .Property(x => x.Name)
                .HasMaxLength(100)
                .IsUnicode(true)
                .IsRequired(true);



            builder
                .Property(x => x.PhoneNumber)
                .HasMaxLength(10)
                .IsFixedLength(true)
                .IsRequired(false)
                .IsUnicode(false);

            builder
                .Property(x => x.Birthday)
                .IsRequired(false);

        }
    }
}
