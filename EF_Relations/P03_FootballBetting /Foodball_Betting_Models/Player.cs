using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P03_FootballBetting
{
    public class Player
    {
        public Player()
        {
            this.PlayerStatistics = new HashSet<PlayerStatistic>();
        }

        public int PlayerId { get; set; }


        public bool Injured { get; set; }


        [Required]
        public string Name { get; set; }

 
        public int PossitionId { get; set; }

        [ForeignKey(nameof(PossitionId))]
        public Position Position { get; set; }


        [Required]
        public int SquadNumber { get; set; }


        public int TeamId { get; set; }

        [ForeignKey(nameof(TeamId))]
        public Team Team { get; set; }


        [InverseProperty("Player")]
        public ICollection<PlayerStatistic> PlayerStatistics { get; set; }
    }
}
