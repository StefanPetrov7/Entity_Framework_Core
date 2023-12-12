using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Importer.Models
{
    /// <summary>
    /// Discribing the Json file which are going to import into our model, Attributes can be used if the prop names are not matching!
    /// </summary>

    public class PropertyJsonModel
    {
        public string Url { get; set; } = null!;

        public int Size { get; set; }

        public int YardSize { get; set; }

        public int Floor { get; set; }

        public int TotalFloor { get; set; }

        public string District { get; set; } = null!;

        public int Year { get; set; }

        public string Type { get; set; } = null!;

        public string BuildingType { get; set; } = null!;

        public int Price { get; set; }
    }
}
