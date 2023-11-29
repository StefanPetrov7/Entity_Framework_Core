﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicHub.Data.Models
{
    public class Song
    {
        public Song()
        {
            this.SongPerformers = new HashSet<SongPerformer>();
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(20)]
        public string? Name { get; set; }

        [Required]
        public TimeSpan Duration { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public Genre Genre { get; set; }

        public int? AlbumId { get; set; }

        [ForeignKey(nameof(AlbumId))]
        public Album? Album { get; set; }

        public int WriterId { get; set; }

        [ForeignKey(nameof(WriterId))]
        public Writer? Writer { get; set; }

        [Required]
        public decimal Price { get; set; }

        [InverseProperty(nameof(SongPerformer.Song))]
        public ICollection<SongPerformer>? SongPerformers { get; set; }


    }
}
