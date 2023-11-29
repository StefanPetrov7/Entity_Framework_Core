﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicHub.Data.Models
{
    public class Performer
    {
        public Performer()
        {
            this.PerformerSongs = new HashSet<SongPerformer>();    
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(20)]
        [Required]
        public string? FirstName { get; set; }

        [MaxLength(20)]
        [Required]
        public string? LastName { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        public decimal NetWorth { get; set; }

        [InverseProperty(nameof(SongPerformer.Performer))]
        public ICollection<SongPerformer>? PerformerSongs { get; set; }


    }
}
