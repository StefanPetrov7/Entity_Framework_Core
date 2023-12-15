using SoftJail.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SoftJail.Data.Models
{
    public class Mail
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public string Sender { get; set; } = null!;

        [Required]
        public string Address { get; set; } = null!;
        public int PrisonerId { get; set; }

        [Required]
        [ForeignKey(nameof(PrisonerId))]
        public Prisoner? Prisoner { get; set; }


    }
}


//Address – text, consisting only of letters, spaces and numbers, which ends with "str." (required) (Example: "62 Muir Hill str.") 

