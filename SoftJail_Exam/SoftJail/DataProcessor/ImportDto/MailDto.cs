using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftJail.DataProcessor.ImportDto
{
    public class MailDto
    {
        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public string Sender { get; set; } = null!;

        [RegularExpression(@"^[a-zA-Z0-9 ]*str\.$")]
        public string Address { get; set; } = null!;
    }
}
