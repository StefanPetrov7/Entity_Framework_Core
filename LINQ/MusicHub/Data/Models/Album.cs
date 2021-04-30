using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace MusicHub.Data.Models
{
    public class Album
    {
        public Album()
        {
            this.Songs = new HashSet<Song>();
        }

   
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string Name { get; set; }

   
        public DateTime ReleaseDate { get; set; }

        public decimal Price => this.Songs.Sum(x => x.Price);

        public int? ProducerId { get; set; }     // Not all Albums will have a producer, that's why we need to make the int?, since by default, all VALUE types are required. 

        [ForeignKey(nameof(ProducerId))]
        public Producer Producer { get; set; }

        [InverseProperty("Album")]
        public ICollection<Song> Songs { get; set; }

    }
}
