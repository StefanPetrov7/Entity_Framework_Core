using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicHub.Data.Models
{
    public class Album
    {
        public Album()
        {
            this.Songs = new HashSet<Song>();
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(40)]
        [Required]
        public string? Name { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        public decimal Price => this.Songs.Sum(x => x.Price);

        public int? ProducerId { get; set; }

        [ForeignKey(nameof(ProducerId))]
        public Producer? Producer { get; set; }

        [InverseProperty(nameof(Song.Album))]
        public ICollection<Song>? Songs { get; set; }
    }
}
