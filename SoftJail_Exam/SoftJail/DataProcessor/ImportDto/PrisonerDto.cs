using SoftJail.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftJail.DataProcessor.ImportDto
{
    public class PrisonerDto
    {
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string FullName { get; set; } = null!;

        [Required]
        [RegularExpression(@"^The [A-Z][a-z]*")]
        public string Nickname { get; set; } = null!;

        [Required]
        [Range(18, 65)]
        public int Age { get; set; }

        [Required]
        public string IncarcerationDate { get; set; } = null!;

        public string? ReleaseDate { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal? Bail { get; set; }

        public int? CellId { get; set; }

        public ICollection<MailDto>? Mails { get; set; }

    }
}