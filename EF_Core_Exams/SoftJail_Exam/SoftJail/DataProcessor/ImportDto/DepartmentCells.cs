using SoftJail.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftJail.DataProcessor.ImportDto
{
    public class DepartmentCells
    {
        [Required]
        [StringLength(25, MinimumLength = 3)]
        public string Name { get; set; } = null!;

        public ICollection<CellDto>? Cells { get; set; }
    }
}
