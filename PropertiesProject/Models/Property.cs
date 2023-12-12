using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Property
    {
        public Property()
        {
            this.Tags = new HashSet<Tag>();
        }

        [Key]
        public int Id { get; set; }

        public int Size { get; set; }

        [AllowNull]
        public int? YardSize { get; set; }

        [AllowNull]
        public byte? Floor { get; set; }

        [AllowNull]
        public byte? TotalFloors { get; set; }

        [AllowNull]
        public int? Year { get; set; }

        public int? Price { get; set; }

        public int DistrictId { get; set; }

        [ForeignKey(nameof(DistrictId))]
        public virtual  District? District { get; set; }

        public int PropertyTypeId { get; set; }

        [ForeignKey(nameof(PropertyTypeId))]
        public virtual PropertyType? PropertyType { get; set; }

        public int BuildingTypeId { get; set; }

        [ForeignKey(nameof(BuildingTypeId))]
        public virtual BuildingType? BuildingType { get; set; }

        [InverseProperty(nameof(Tag.Properties))]
        public virtual ICollection<Tag>? Tags { get; set; }

    }
}
