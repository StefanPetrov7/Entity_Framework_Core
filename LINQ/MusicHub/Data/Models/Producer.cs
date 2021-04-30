using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicHub.Data.Models
{
    public class Producer
    {
        public Producer()
        {
            this.Albums = new HashSet<Album>();
        }


        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        public string Pseudonym { get; set; }    // string data type by default will translate into SQL as a NVARCHAR(MAX) / ALLOW NULL  

        public string PhoneNumber { get; set; }

        [InverseProperty("Producer")]
        public ICollection<Album> Albums { get; set; }
    }
}
