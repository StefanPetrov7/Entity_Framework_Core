using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class PropertyType
    {
        public PropertyType()
        {
            this.Properties = new HashSet<Property>(); 
        }

        [Key]
        public int MyProperty { get; set; }

        public string Name { get; set; } = null!;

        [InverseProperty(nameof(Property.PropertyType))]
        public virtual ICollection<Property>? Properties { get; set; }
    }
}
