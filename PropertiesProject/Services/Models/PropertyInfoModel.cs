using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public class PropertyInfoModel
    {
        public string DistrictName { get; set; } = null!;

        public int Size { get; set; }

        public int Price { get; set; }

        public string PropertyType { get; set; } = null!;

        public string BuildingType { get; set; } = null!; 
    }
}


