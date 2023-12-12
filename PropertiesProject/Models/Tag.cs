using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Tag
    {
        public Tag()
        {
            this.Properties = new HashSet<Property>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public int? Importance { get; set; }

        [InverseProperty(nameof(Property.Tags))]
        public virtual  ICollection<Property>? Properties { get; set; }
    }
}
