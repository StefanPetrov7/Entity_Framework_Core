using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public class DistrictInfoModel
    {
        public string Name { get; set; } = null!;

        public decimal AvgPricePerM2 { get; set; }

        public int PropertiesCount { get; set; }
    }
}
