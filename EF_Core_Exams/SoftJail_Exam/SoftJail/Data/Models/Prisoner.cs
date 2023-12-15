using SoftJail.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftJail.Data.Models
{
    public class Prisoner
    {
        public Prisoner()
        {
            this.Mails = new HashSet<Mail>();
            this.PrisonerOfficers = new HashSet<OfficerPrisoner>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string FullName { get; set; } = null!;

        [Required]
        public string Nickname { get; set; } = null!;

        [Required]
        [Range(18, 65)]
        public int Age { get; set; }

        [Required]
        public DateTime IncarcerationDate { get; set; }

    
        public DateTime? ReleaseDate { get; set; }    // Not mentoined that it is nullable in the problem disription 

        [Range(typeof(decimal), "0", "79228162514264337593543950335")] 
        public decimal? Bail { get; set; }    // Not mentoined that it is nullable in the problem disription 


        public int? CellId { get; set; }    // Not mentoined that it is nullable in the problem disription 

        [ForeignKey(nameof(CellId))]
        public Cell Cell { get; set; }


        public ICollection<Mail> Mails { get; set; }


        public ICollection<OfficerPrisoner> PrisonerOfficers { get; set; }

    }
}