using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MusicHub.Data.Models.Enums;

namespace MusicHub.Data.Models
{
    public class Song
    {
        public Song()
        {
            this.SongPerformers = new HashSet<SongPerformer>();
        }

     
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

 
        public TimeSpan Duration  { get; set; }



        public DateTime CreatedOn { get; set; }


   
        public Genre Genre { get; set; }



        public int? AlbumId { get; set; }    // Can be NULL as not every song will be a part of an Album. VALUE Data Type which is not required '?'.

        [ForeignKey(nameof(AlbumId))]
        public Album Album { get; set; }

        [Required]
        public int WriterId { get; set; }

        [ForeignKey(nameof(WriterId))]
        public Writer Writer { get; set; }

        

        public decimal Price { get; set; }

        [InverseProperty("Song")]
        public ICollection<SongPerformer> SongPerformers { get; set; }
    }
}
