using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftJail.DataProcessor.ExportDto
{
    public class ExPrisonerDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;   

        public int CellNumber { get; set; }

        public ICollection<ExOfficerDto>? Officers { get; set; }

        public decimal TotalOfficerSalary { get; set; }
    }
}
