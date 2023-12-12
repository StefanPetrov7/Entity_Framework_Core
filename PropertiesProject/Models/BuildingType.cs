using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class BuildingType
    {
        public BuildingType()
        {
            this.Properties = new HashSet<Property>();
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        [InverseProperty(nameof(Property.BuildingType))]
        public virtual ICollection<Property>? Properties { get; set; }
    }
}
